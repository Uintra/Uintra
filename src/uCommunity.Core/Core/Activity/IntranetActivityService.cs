using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Activity.Entities;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Extentions;
using Umbraco.Core.Models;

namespace uCommunity.Core.Activity
{
    public abstract class IntranetActivityService<TActivity> : IIntranetActivityService<TActivity> where TActivity : IIntranetActivity
    {
        public abstract IntranetActivityTypeEnum ActivityType { get; }

        protected DateTimeOffset CacheExpirationOffset { get; } = DateTimeOffset.Now.AddDays(1);

        private const string CacheKey = "ActivityCache";
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly ICacheService _cache;


        protected IntranetActivityService(IIntranetActivityRepository activityRepository,
            ICacheService cache)
        {
            _activityRepository = activityRepository;
            _cache = cache;
        }

        public TActivity Get(Guid id, bool includeHidden = false)
        {
            var cached = GetAll(includeHidden).FirstOrDefault(s => s.Id == id);
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

        public Guid Create(IIntranetActivity jsonData)
        {
            var newActivity = new IntranetActivityEntity { Type = ActivityType, JsonData = jsonData.ToJson() };
            _activityRepository.Create(newActivity);

            var newActivityId = newActivity.Id;
            UpdateCachedEntity(newActivityId);
            return newActivityId;
        }

        public void Save(IIntranetActivity saveModel)
        {
            var saveModelId = saveModel.Id;
            var activity = _activityRepository.Get(saveModelId);
            activity.JsonData = saveModel.ToJson();
            _activityRepository.Update(activity);
            UpdateCachedEntity(saveModelId);
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
            var activities = _cache.GetOrSet(CacheKey, GetAllForCache, CacheExpirationOffset, $"{ActivityType}");
            return activities;
        }

        protected TActivity UpdateCachedEntity(Guid id)
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
            _cache.Set(CacheKey, cachedList, CacheExpirationOffset, $"{ActivityType}");

            return cachedActivity;
        }

        private IList<TActivity> GetAllForCache()
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
            return cachedActivity;
        }

        protected abstract void MapBeforeCache(IList<IIntranetActivity> cached);
    }
}