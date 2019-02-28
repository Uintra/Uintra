using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;

namespace Uintra.Core.Activity
{
    public abstract class IntranetActivityService<TActivity> : IIntranetActivityService<TActivity>, ICacheableIntranetActivityService<TActivity> where TActivity : IIntranetActivity
    {
        public abstract Enum Type { get; }
        public abstract PermissionResourceTypeEnum PermissionActivityType { get; }
        private const string CacheKey = "ActivityCache";
        private string ActivityCacheSuffix => $"{Type.ToString()}";
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly ICacheService _cache;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IActivityLocationService _activityLocationService;
        private readonly IActivityLinkPreviewService _activityLinkPreviewService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IPermissionsService _permissionsService;

        protected IntranetActivityService(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
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

        protected abstract void MapBeforeCache(IList<TActivity> cached);

    }
}