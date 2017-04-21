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
    public abstract class IntranetActivityService<TCachedActivity> : IIntranetActivityService<TCachedActivity> where TCachedActivity : IntranetActivity
    {
        public abstract IntranetActivityTypeEnum ActivityType { get; }

        public DateTimeOffset CacheExpirationOffset { get; } = DateTimeOffset.Now.AddDays(1);

        private const string CacheKey = "ActivityCache";
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly ICacheService _cache;


        protected IntranetActivityService(IIntranetActivityRepository activityRepository,
            ICacheService cache)
        {
            _activityRepository = activityRepository;
            _cache = cache;
        }

        public TCachedActivity Get(Guid id)
        {
            var cached = GetAll().FirstOrDefault(s => s.Id == id);
            return cached;
        }

        public IEnumerable<TCachedActivity> GetManyActual()
        {
            var cached = GetAll(true);
            var actual = cached.Where(IsActual);
            return actual;
        }

        public IEnumerable<TCachedActivity> FindAll(Func<TCachedActivity, bool> predicate)
        {
            var cached = GetAll().Where(predicate);
            return cached;
        }

        public IEnumerable<TCachedActivity> GetAll(bool includeHidden = false)
        {
            var cached = GetAllFromCache();
            if (!includeHidden)
            {
                cached = cached.Where(s => !s.IsHidden);
            }
            return cached;
        }

        public virtual bool IsActual(TCachedActivity cachedActivity)
        {
            return !cachedActivity.IsHidden;
        }

        public Guid Create(TCachedActivity jsonData)
        {
            var newActivity = new IntranetActivityEntity { Type = ActivityType, JsonData = jsonData.ToJson() };
            _activityRepository.Create(newActivity);

            var newActivityId = newActivity.Id;
            UpdateCachedEntity(newActivityId);
            return newActivityId;
        }

        public void Save(TCachedActivity saveModel)
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

        public abstract bool CanEdit(TCachedActivity cached);
        public abstract IPublishedContent GetOverviewPage();
        public abstract IPublishedContent GetDetailsPage();
        public abstract IPublishedContent GetCreatePage();
        public abstract IPublishedContent GetEditPage();
        public abstract IPublishedContent GetOverviewPage(IPublishedContent currentPage);
        public abstract IPublishedContent GetDetailsPage(IPublishedContent currentPage);
        public abstract IPublishedContent GetCreatePage(IPublishedContent currentPage);
        public abstract IPublishedContent GetEditPage(IPublishedContent currentPage);

        protected IEnumerable<TCachedActivity> GetAllFromCache()
        {
            var activities = _cache.GetOrSet(CacheKey, GetAllForCache, CacheExpirationOffset, $"{ActivityType}");
            return activities;
        }

        protected void UpdateCachedEntity(Guid id)
        {
            var activity = _activityRepository.Get(id);
            var cached = GetAll(true);
            var cachedList = cached as List<TCachedActivity> ?? cached.ToList();

            if (activity == null)
            {
                cachedList = cachedList.FindAll(s => s.Id != id);
            }
            else
            {
                var cachedActivity = MapInternal(activity);
                MapBeforeCache(Enumerable.Repeat(cachedActivity, 1).ToList());
                cachedList.Add(cachedActivity);
            }
            _cache.Set(CacheKey, cachedList, CacheExpirationOffset, $"{ActivityType}");
        }

        private IList<TCachedActivity> GetAllForCache()
        {
            var activities = _activityRepository.GetMany(ActivityType).Select(MapInternal).ToList();
            MapBeforeCache(activities);
            return activities;
        }

        private TCachedActivity MapInternal(IntranetActivityEntity activity)
        {
            var cachedActivity = activity.JsonData.Deserialize<TCachedActivity>();
            cachedActivity.Id = activity.Id;
            cachedActivity.Type = activity.Type;
            cachedActivity.CreatedDate = activity.CreatedDate;
            cachedActivity.ModifyDate = activity.ModifyDate;
            return cachedActivity;
        }

        protected abstract void MapBeforeCache(IList<TCachedActivity> cached);
    }
}