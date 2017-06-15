using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;

namespace uIntra.Core.Activity
{
    public abstract class IntranetActivityService<TActivity> : IIntranetActivityService<TActivity> where TActivity : IIntranetActivity
    {
        public abstract IntranetActivityTypeEnum ActivityType { get; }

        private const string CacheKey = "ActivityCache";
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly ICacheService _cache;

        protected IntranetActivityService(IIntranetActivityRepository activityRepository,
            ICacheService cache)
        {
            _activityRepository = activityRepository;
            _cache = cache;
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
            var cached = GetAllFromCache();
            if (!includeHidden)
            {
                cached = cached.Where(s => !s.IsHidden);
            }
            return cached;
        }

        public virtual bool IsActual(IIntranetActivity cachedActivity)
        {
            return !cachedActivity.IsHidden;
        }

        public Guid Create(IIntranetActivity activity)
        {
            var newActivity = new IntranetActivityEntity { Type = ActivityType, JsonData = activity.ToJson() };
            _activityRepository.Create(newActivity);

            var newActivityId = newActivity.Id;
            UpdateCachedEntity(newActivityId);
            return newActivityId;
        }

        public void Save(IIntranetActivity activity)
        {
            var entity = _activityRepository.Get(activity.Id);
            entity.JsonData = activity.ToJson();
            _activityRepository.Update(entity);
            UpdateCachedEntity(activity.Id);
        }

        public void Delete(Guid id)
        {
            _activityRepository.Delete(id);
            UpdateCachedEntity(id);
        }

        public bool CanEdit(Guid id)
        {
            var cached = Get(id);
            return CanEdit(cached);
        }

        public abstract bool CanEdit(IIntranetActivity cached);
        public abstract IPublishedContent GetOverviewPage();
        public abstract IPublishedContent GetDetailsPage();
        public abstract IPublishedContent GetCreatePage();
        public abstract IPublishedContent GetEditPage();
        public abstract IPublishedContent GetOverviewPage(IPublishedContent currentPage);
        public abstract IPublishedContent GetDetailsPage(IPublishedContent currentPage);
        public abstract IPublishedContent GetCreatePage(IPublishedContent currentPage);
        public abstract IPublishedContent GetEditPage(IPublishedContent currentPage);

        protected IEnumerable<TActivity> GetAllFromCache()
        {
            var activities = _cache.GetOrSet(CacheKey, GetAllFromSql, CacheHelper.GetDateTimeOffsetToMidnight(), $"{ActivityType}");
            return activities;
        }

        protected virtual TActivity UpdateCachedEntity(Guid id)
        {
            var activity = _activityRepository.Get(id);
            var cached = GetAll(true);
            var cachedList = (cached as List<TActivity> ?? cached.ToList()).FindAll(s => s.Id != id);
            var cachedActivity = default(TActivity);

            if (activity != null)
            {
                cachedActivity = MapInternal(activity);
                MapBeforeCache(Enumerable.Repeat((IIntranetActivity)cachedActivity, 1).ToList());
                cachedList.Add(cachedActivity);
            }

            _cache.Set(CacheKey, cachedList, CacheHelper.GetDateTimeOffsetToMidnight(), $"{ActivityType}");

            return cachedActivity;
        }

        private TActivity GetFromSql(Guid id)
        {
            var activity = MapInternal(_activityRepository.Get(id));
            return activity;
        }

        private IList<TActivity> GetAllFromSql()
        {
            var activities = _activityRepository.GetMany(ActivityType).Select(MapInternal).ToList();
            MapBeforeCache(activities.Select(s => (IIntranetActivity)s).ToList());
            return activities;
        }

        private TActivity MapInternal(IntranetActivityEntity activity)
        {
            var cachedActivity = activity.JsonData.Deserialize<TActivity>();
            cachedActivity.Id = activity.Id;
            cachedActivity.Type = activity.Type;
            cachedActivity.CreatedDate = activity.CreatedDate;
            cachedActivity.ModifyDate = activity.ModifyDate;
            cachedActivity.IsPinActual = IsPinActual(cachedActivity);
            return cachedActivity;
        }

        private bool IsPinActual(IIntranetActivity activity)
        {
            if (!activity.IsPinned) return false;

            if (activity.EndPinDate.HasValue)
            {
                return activity.EndPinDate.Value.ToUniversalTime() > DateTime.UtcNow;
            }

            return true;
        }

        protected abstract void MapBeforeCache(IList<IIntranetActivity> cached);
    }
}