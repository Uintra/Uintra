using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using UBaseline.Core.Extensions;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Sql;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.LinkPreview;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Activity
{
    public abstract class IntranetActivityService<TActivity> : IIntranetActivityService<TActivity>, ICacheableIntranetActivityService<TActivity> where TActivity : IIntranetActivity
    {
        public abstract Enum Type { get; }
        public abstract Enum PermissionActivityType { get; }
        private const string CacheKey = "ActivityCache";
        private string ActivityCacheSuffix => $"{Type.ToString()}";
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly ICacheService _cache;//TODO: Implement async methods at cache service
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IActivityLocationService _activityLocationService;
        private readonly IActivityLinkPreviewService _activityLinkPreviewService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IPermissionsService _permissionsService;

        protected IntranetActivityService(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IPermissionsService permissionsService)
        {
            _activityRepository = activityRepository;
            _cache = cache;
            _activityTypeProvider = activityTypeProvider;
            _intranetMediaService = intranetMediaService;
            _activityLocationService = activityLocationService;
            _activityLinkPreviewService = activityLinkPreviewService;
            _intranetMemberService = intranetMemberService;
            _permissionsService = permissionsService;
        }

        #region async

        public async Task<TActivity> GetAsync(Guid id)
        {
            var cached = (await GetAllAsync(true)).SingleOrDefault(s => s.Id == id);
            return cached;
        }

        public async Task<IEnumerable<TActivity>> GetManyActualAsync()
        {
            var cached = await GetAllAsync(true);
            var actual = cached.Where(s => IsActual(s));
            return actual;
        }

        public async Task<IEnumerable<TActivity>> GetAllAsync(bool includeHidden = false)
        {
            if (!_cache.HasValue(CacheKey, ActivityCacheSuffix))
            {
                await UpdateCacheAsync();
            }
            var cached = GetAllFromCache();
            if (!includeHidden)
            {
                cached = cached.Where(s => !s.IsHidden);
            }
            return cached;
        }

        public virtual async Task<Guid> CreateAsync(IIntranetActivity activity) => await CreateAsync(activity, null);

        public virtual async Task SaveAsync(IIntranetActivity activity) => await SaveAsync(activity, null);

        public virtual async Task DeleteAsync(Guid id)
        {
            await _activityLocationService.DeleteForActivityAsync(id);
            await _activityLinkPreviewService.RemovePreviewRelationsAsync(id);
            await _activityRepository.DeleteAsync(id);
            await _intranetMediaService.DeleteAsync(id);

            await UpdateActivityCacheAsync(id);
        }

        public async Task<bool> CanEditAsync(Guid id)
        {
            var cached = await GetAsync(id);
            return await CanEditAsync(cached);
        }

        public async Task<bool> CanDeleteAsync(Guid id)
        {
            var cached = await GetAsync(id);
            return await CanDeleteAsync(cached);
        }

        public virtual async Task<bool> CanEditAsync(IIntranetActivity activity) =>
            await CanPerformAsync(activity, PermissionActionEnum.Edit, PermissionActionEnum.EditOther);

        public virtual async Task<bool> CanDeleteAsync(IIntranetActivity activity) =>
            await CanPerformAsync(activity, PermissionActionEnum.Delete, PermissionActionEnum.DeleteOther);

        public virtual async Task<TActivity> UpdateActivityCacheAsync(Guid activityId)
        {
            var activity = await GetFromSqlAsync(activityId);
            var cached = await GetAllAsync(true);
            var cachedList = (cached as List<TActivity> ?? cached.ToList()).FindAll(s => s.Id != activityId);

            if (activity != null)
            {
                await MapBeforeCacheAsync(activity.ToListOfOne());
                cachedList.Add(activity);
            }

            await _cache.SetAsync(() => cachedList.AsTask(), CacheKey, CacheHelper.GetMidnightUtcDateTimeOffset(), ActivityCacheSuffix);

            return activity;
        }

        protected virtual async Task UpdateCacheAsync()
        {
            await FillCacheAsync();
        }

        protected virtual async Task<Guid> CreateAsync(IIntranetActivity activity, Action<Guid> afterCreateAction)
        {
            var newActivity = new IntranetActivityEntity { Type = Type.ToInt(), JsonData = activity.ToJson() };
            await _activityRepository.CreateAsync(newActivity);
            var newActivityId = newActivity.Id;

            await _activityLocationService.SetAsync(newActivityId, activity.Location);
            await _intranetMediaService.CreateAsync(newActivityId, activity.MediaIds.JoinToString());
            await AssignLinkPreviewAsync(newActivityId, activity);

            afterCreateAction?.Invoke(newActivityId);

            await UpdateActivityCacheAsync(newActivityId);
            return newActivityId;
        }

        protected virtual async Task SaveAsync(IIntranetActivity activity, Action<IIntranetActivity> afterSaveAction)
        {
            var entity = await _activityRepository.GetAsync(activity.Id);
            entity.JsonData = activity.ToJson();

            await _activityLocationService.SetAsync(activity.Id, activity.Location);
            await _activityRepository.UpdateAsync(entity);
            await _intranetMediaService.UpdateAsync(activity.Id, activity.MediaIds.JoinToString());
            await AssignLinkPreviewAsync(activity);

            afterSaveAction?.Invoke(activity);
            await UpdateActivityCacheAsync(activity.Id);
        }

        private async Task FillCacheAsync()
        {
            await _cache.SetAsync(GetAllFromSqlAsync, CacheKey, CacheHelper.GetMidnightUtcDateTimeOffset(), ActivityCacheSuffix);
        }

        private async Task<TActivity> GetFromSqlAsync(Guid id)
        {
            var activityEntity = await _activityRepository.GetAsync(id);
            if (activityEntity == null)
            {
                return default(TActivity);
            }

            var activity = await MapInternalAsync(activityEntity);
            return activity;
        }

        private async Task<IList<TActivity>> GetAllFromSqlAsync()
        {
            var activities = await (await _activityRepository.GetManyAsync(Type)).SelectAsync(async model => await MapInternalAsync(model));
            await MapBeforeCacheAsync(activities.ToList());
            return activities.ToList();
        }

        protected virtual async Task<bool> CanPerformAsync(IIntranetActivity activity, PermissionActionEnum action, PermissionActionEnum administrationAction)
        {
            //var currentMember = await _intranetMemberService.GetCurrentMemberAsync();
            var currentMember = _intranetMemberService.GetCurrentMember();
            var ownerId = ((IHaveOwner)activity).OwnerId;
            var isOwner = ownerId == currentMember.Id;

            var act = isOwner ? action : administrationAction;
            var result = await _permissionsService.CheckAsync(currentMember, PermissionSettingIdentity.Of(act, PermissionActivityType));

            return result;
        }

        protected virtual async Task AssignLinkPreviewAsync(Guid newActivityId, IIntranetActivity activity)
        {
            if (activity is IHasLinkPreview linkPreview)
            {
                if (linkPreview.LinkPreviewId.HasValue)
                {
                    await _activityLinkPreviewService.AddLinkPreviewAsync(newActivityId, linkPreview.LinkPreviewId.Value);
                }
            }
        }

        protected virtual async Task AssignLinkPreviewAsync(IIntranetActivity activity)
        {
            if (activity is IHasLinkPreview linkPreview)
            {
                if (!linkPreview.LinkPreviewId.HasValue)
                {
                    await _activityLinkPreviewService.RemovePreviewRelationsAsync(activity.Id);
                }

                if (linkPreview.LinkPreviewId.HasValue)
                {
                    await _activityLinkPreviewService.UpdateLinkPreviewAsync(activity.Id, linkPreview.LinkPreviewId.Value);
                }
            }
        }

        #endregion

        #region sync

        public TActivity Get(Guid id)
        {
            var cached = GetAll(true).SingleOrDefault(s => s.Id == id);
            return cached;
        }

        public IEnumerable<TActivity> GetManyActual()
        {
            var cached = GetAll(true);
            var actual = cached.Where(s => IsActual(s));
            return actual;
        }

        public IEnumerable<TActivity> GetAll(bool includeHidden = false)
        {
            if (!_cache.HasValue(CacheKey, ActivityCacheSuffix))
            {
                UpdateCache();
            }
            var cached = GetAllFromCache();
            if (!includeHidden)
            {
                cached = cached.Where(s => !s.IsHidden);
            }
            return cached;
        }

        protected virtual void UpdateCache()
        {
            FillCache();
        }

        private void FillCache()
        {
            var items = GetAllFromSql();
            _cache.Set(CacheKey, items, CacheHelper.GetMidnightUtcDateTimeOffset(), ActivityCacheSuffix);
        }

        public virtual bool IsActual(IIntranetActivity activity)
        {
            return !activity.IsHidden;
        }

        public virtual bool IsPinActual(IIntranetActivity activity)
        {
            if (!activity.IsPinned) return false;

            if (activity.EndPinDate.HasValue)
            {
                return activity.EndPinDate.Value.ToUniversalTime() > DateTime.UtcNow;
            }

            return true;
        }

        public virtual Guid Create(IIntranetActivity activity) => Create(activity, null);

        protected virtual Guid Create(IIntranetActivity activity, Action<Guid> afterCreateAction)
        {
            var newActivity = new IntranetActivityEntity { Type = Type.ToInt(), JsonData = activity.ToJson() };
            _activityRepository.Create(newActivity);
            var newActivityId = newActivity.Id;

            _activityLocationService.Set(newActivityId, activity.Location);
            _intranetMediaService.Create(newActivityId, activity.MediaIds.JoinToString());
            AssignLinkPreview(newActivityId, activity);

            afterCreateAction?.Invoke(newActivityId);

            UpdateActivityCache(newActivityId);
            return newActivityId;
        }

        public virtual void Save(IIntranetActivity activity) => Save(activity, null);

        protected virtual void Save(IIntranetActivity activity, Action<IIntranetActivity> afterSaveAction)
        {
            var entity = _activityRepository.Get(activity.Id);
            entity.JsonData = activity.ToJson();

            _activityLocationService.Set(activity.Id, activity.Location);
            _activityRepository.Update(entity);
            _intranetMediaService.Update(activity.Id, activity.MediaIds.JoinToString());
            AssignLinkPreview(activity);

            afterSaveAction?.Invoke(activity);
            UpdateActivityCache(activity.Id);
        }

        public virtual void Delete(Guid id)
        {
            _activityLocationService.DeleteForActivity(id);
            _activityLinkPreviewService.RemovePreviewRelations(id);
            _activityRepository.Delete(id);
            _intranetMediaService.Delete(id);

            UpdateActivityCache(id);
        }

        public bool CanEdit(Guid id)
        {
            var cached = Get(id);
            return CanEdit(cached);
        }

        public bool CanDelete(Guid id)
        {
            var cached = Get(id);
            return CanDelete(cached);
        }

        public virtual bool CanEdit(IIntranetActivity activity) =>
            CanPerform(activity, PermissionActionEnum.Edit, PermissionActionEnum.EditOther);

        public virtual bool CanDelete(IIntranetActivity activity) =>
            CanPerform(activity, PermissionActionEnum.Delete, PermissionActionEnum.DeleteOther);

        protected virtual bool CanPerform(IIntranetActivity activity, PermissionActionEnum action, PermissionActionEnum administrationAction)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var ownerId = ((IHaveOwner)activity).OwnerId;
            var isOwner = ownerId == currentMember.Id;

            var act = isOwner ? action : administrationAction;
            var result = _permissionsService.Check(currentMember, PermissionSettingIdentity.Of(act, PermissionActivityType));

            return result;
        }

        protected IEnumerable<TActivity> GetAllFromCache()
        {
            var activities = _cache.Get<IList<TActivity>>(CacheKey, ActivityCacheSuffix);
            return activities;
        }

        public virtual TActivity UpdateActivityCache(Guid id)
        {
            var activity = GetFromSql(id);
            var cached = GetAll(true);
            var cachedList = (cached as List<TActivity> ?? cached.ToList()).FindAll(s => s.Id != id);

            if (activity != null)
            {
                MapBeforeCache(activity.ToListOfOne());
                cachedList.Add(activity);
            }

            _cache.Set(CacheKey, cachedList, CacheHelper.GetMidnightUtcDateTimeOffset(), ActivityCacheSuffix);

            return activity;
        }

        protected virtual void AssignLinkPreview(Guid newActivityId, IIntranetActivity activity)
        {
            if (activity is IHasLinkPreview linkPreview)
            {
                if (linkPreview.LinkPreviewId.HasValue)
                {
                    _activityLinkPreviewService.AddLinkPreview(newActivityId, linkPreview.LinkPreviewId.Value);
                }
            }
        }

        protected virtual void AssignLinkPreview(IIntranetActivity activity)
        {
            if (activity is IHasLinkPreview linkPreview)
            {
                if (!linkPreview.LinkPreviewId.HasValue)
                {
                    _activityLinkPreviewService.RemovePreviewRelations(activity.Id);
                }

                if (linkPreview.LinkPreviewId.HasValue)
                {
                    _activityLinkPreviewService.UpdateLinkPreview(activity.Id, linkPreview.LinkPreviewId.Value);
                }
            }
        }

        private TActivity GetFromSql(Guid id)
        {
            var activityEntity = _activityRepository.Get(id);
            if (activityEntity == null)
            {
                return default(TActivity);
            }

            var activity = MapInternal(activityEntity);
            return activity;
        }

        private IList<TActivity> GetAllFromSql()
        {
            var activities = _activityRepository.GetMany(Type).Select(MapInternal).ToList();
            MapBeforeCache(activities.ToList());
            return activities;
        }

        private TActivity MapInternal(IntranetActivityEntity activity)
        {
            var cachedActivity = activity.JsonData.Deserialize<TActivity>();
            cachedActivity.Id = activity.Id;
            cachedActivity.Type = _activityTypeProvider[activity.Type];
            cachedActivity.CreatedDate = activity.CreatedDate;
            cachedActivity.ModifyDate = activity.ModifyDate;
            cachedActivity.IsPinActual = IsPinActual(cachedActivity);
            cachedActivity.MediaIds = _intranetMediaService.GetEntityMedia(cachedActivity.Id);
            cachedActivity.Location = _activityLocationService.Get(activity.Id);
            return cachedActivity;
        }

        private async Task<TActivity> MapInternalAsync(IntranetActivityEntity activity)
        {
            var cachedActivity = activity.JsonData.Deserialize<TActivity>();
            cachedActivity.Id = activity.Id;
            cachedActivity.Type = _activityTypeProvider[activity.Type];
            cachedActivity.CreatedDate = activity.CreatedDate;
            cachedActivity.ModifyDate = activity.ModifyDate;
            cachedActivity.IsPinActual = IsPinActual(cachedActivity);
            cachedActivity.MediaIds = await _intranetMediaService.GetEntityMediaAsync(cachedActivity.Id);
            cachedActivity.Location = await _activityLocationService.GetAsync(activity.Id);
            return cachedActivity;
        }

        protected abstract void MapBeforeCache(IList<TActivity> cached);

        protected abstract Task MapBeforeCacheAsync(IList<TActivity> cached);

        #endregion

    }
}