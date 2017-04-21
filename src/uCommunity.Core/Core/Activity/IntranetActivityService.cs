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

        public TActivity Get<TActivity>(Guid id) where TActivity: TCachedActivity
        {
            var cached = GetAll<TActivity>().FirstOrDefault(s => s.Id == id);
            return cached;
        }

        public IEnumerable<TActivity> GetManyActual<TActivity>() where TActivity : TCachedActivity
        {
            var cached = GetAll<TActivity>(true);
            var actual = cached.Where(IsActual);
            return actual;
        }

        public IEnumerable<TActivity> FindAll<TActivity>(Func<TActivity, bool> predicate) where TActivity : TCachedActivity
        {
            var cached = GetAll<TActivity>().Where(predicate);
            return cached;
        }

        public IEnumerable<TActivity> GetAll<TActivity>(bool includeHidden = false) where TActivity : TCachedActivity
        {
            var cached = GetAllFromCache<TActivity>();
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
            UpdateCachedEntity<TCachedActivity>(newActivityId);
            return newActivityId;
        }

        public void Save(TCachedActivity saveModel)
        {
            var saveModelId = saveModel.Id;
            var activity = _activityRepository.Get(saveModelId);
            activity.JsonData = saveModel.ToJson();
            _activityRepository.Update(activity);
            UpdateCachedEntity<TCachedActivity>(saveModelId);
        }

        public void Delete(Guid id)
        {
            _activityRepository.Delete(id);
            UpdateCachedEntity<TCachedActivity>(id);
        }

        public bool CanEdit(Guid id)
        {
            var cached = Get<TCachedActivity>(id);
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

        protected IEnumerable<TActivity> GetAllFromCache<TActivity>() where TActivity: TCachedActivity
        {
            var activities = _cache.GetOrSet(CacheKey, GetAllForCache<TActivity>, CacheExpirationOffset, $"{ActivityType}");
            return activities;
        }

        protected TActivity UpdateCachedEntity<TActivity>(Guid id) where TActivity: TCachedActivity
        {
            var activity = _activityRepository.Get(id);
            var cached = GetAll<TCachedActivity>(true);
            var cachedList = cached as List<TCachedActivity> ?? cached.ToList();
            TActivity cachedActivity = null;
            if (activity == null)
            {
                cachedList = cachedList.FindAll(s => s.Id != id);
            }
            else
            {
                cachedActivity = MapInternal<TActivity>(activity);
                MapBeforeCache(Enumerable.Repeat(cachedActivity, 1).ToList());
                cachedList.Add(cachedActivity);
            }
            _cache.Set(CacheKey, cachedList, CacheExpirationOffset, $"{ActivityType}");

            return cachedActivity;
        }

        private IList<TActivity> GetAllForCache<TActivity>() where TActivity : TCachedActivity
        {
            var activities = _activityRepository.GetMany(ActivityType).Select(MapInternal<TActivity>).ToList();
            MapBeforeCache(activities);
            return activities;
        }

        private TActivity MapInternal<TActivity>(IntranetActivityEntity activity) where TActivity : TCachedActivity
        {
            var cachedActivity = activity.JsonData.Deserialize<TActivity>();
            cachedActivity.Id = activity.Id;
            cachedActivity.Type = activity.Type;
            cachedActivity.CreatedDate = activity.CreatedDate;
            cachedActivity.ModifyDate = activity.ModifyDate;
            return cachedActivity;
        }

        protected abstract void MapBeforeCache<TActivity>(IList<TActivity> cached) where TActivity : TCachedActivity;
    }
}