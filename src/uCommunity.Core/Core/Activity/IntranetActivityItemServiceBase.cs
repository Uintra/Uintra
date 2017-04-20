using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using uCommunity.Core.Activity.Entities;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Extentions;
using Umbraco.Core.Models;

namespace uCommunity.Core.Activity
{
    public abstract class IntranetActivityItemServiceBase<T, TModel> : IIntranetActivityItemServiceBase<T, TModel>
        where T : IntranetActivityBase, new()
        where TModel : IntranetActivityModelBase, new()
    {
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly ICacheService _cache;

        protected abstract List<string> OverviewXPath { get; }

        public abstract IntranetActivityTypeEnum ActivityType { get; }

        protected IntranetActivityItemServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache)
        {
            _activityRepository = activityRepository;
            _cache = cache;
        }

        public TModel Get(Guid id)
        {
            var activity = GetAll().SingleOrDefault(a => a.Id == id);
            return activity;
        }

        public IEnumerable<TModel> GetManyActual()
        {
            var activities = GetAll();
            return activities.Where(el => IsActual(el as T));
        }

        public IEnumerable<TModel> GetAll(bool includeHidden = false)
        {
            var activities = _cache.GetOrSet(CacheConstants.ActivityCacheKey, () => GetAllFromSql().ToList(), GetCacheExpiration(), ActivityType.ToString());

            if (!includeHidden)
            {
                activities = activities.FindAll(activity => !activity.IsHidden);
            }

            return activities;
        }

        public virtual bool IsActual(T activity)
        {
            return !activity.IsHidden;
        }

        public Guid Create(T model)
        {
            var entity = FillPropertiesOnCreate(model);

            _activityRepository.Create(entity);
            FillCache(entity.Id);
            return entity.Id;
        }

        public void Save(T model)
        {
            var entity = _activityRepository.Get(model.Id);

            FillPropertiesOnEdit(entity, model);

            _activityRepository.Update(entity);
            FillCache(entity.Id);
        }

        public virtual void Delete(Guid id)
        {
            _activityRepository.Delete(id);
            FillCache(id);
        }

        public virtual TModel FillCache(Guid id)
        {
            var activity = GetFromSql(id);

            var activities = GetAll(true).ToList();
            activities = activities.FindAll(a => a.Id != id);
            if (activity != null)
            {
                activities.Add(activity);
            }

            _cache.Set(CacheConstants.ActivityCacheKey, activities, GetCacheExpiration(), ActivityType.ToString());
            return activity;
        }

        protected string[] GetPath(params string[] aliases)
        {
            var basePath = OverviewXPath;

            if (aliases.Any())
            {
                basePath.AddRange(aliases.ToList());
            }
            return basePath.ToArray();
        }

        public abstract IPublishedContent GetOverviewPage();

        public abstract IPublishedContent GetCreatePage();

        public abstract IPublishedContent GetEditPage();

        public abstract IPublishedContent GetDetailsPage();

        public abstract IPublishedContent GetOverviewPage(IPublishedContent currentPage);

        public abstract IPublishedContent GetDetailsPage(IPublishedContent currentPage);

        public abstract IPublishedContent GetCreatePage(IPublishedContent currentPage);

        public abstract IPublishedContent GetEditPage(IPublishedContent currentPage);

        public abstract bool CanEdit(T activity);

        protected virtual IntranetActivityEntity FillPropertiesOnCreate(T model)
        {
            var entity = new IntranetActivityEntity
            {
                Id = Guid.NewGuid(),
                Type = ActivityType
            };
            entity.CreatedDate = entity.ModifyDate = DateTime.Now;
            entity.JsonData = StringExtensions.ToJson(model);
            return entity;
        }

        protected virtual void FillPropertiesOnEdit(IntranetActivityEntity entity, T model)
        {
            entity.ModifyDate = DateTime.Now;
            entity.JsonData = StringExtensions.ToJson(model);
        }

        protected virtual TModel FillPropertiesOnGet(IntranetActivityEntity entity)
        {
            var model = entity.JsonData.Deserialize<TModel>();
            model.Id = entity.Id;
            model.Type = entity.Type;
            model.CreatedDate = entity.CreatedDate;
            model.ModifyDate = entity.ModifyDate;

            return model;
        }

        protected TModel GetFromSql(Guid id)
        {
            var activity = _activityRepository.Get(id);
            if (activity == null)
            {
                return null;
            }

            return GetMany(Enumerable.Repeat(activity, 1)).Single();
        }

        protected IEnumerable<TModel> GetAllFromSql()
        {
            var activities = _activityRepository.GetMany(ActivityType).ToList();
            return GetMany(activities);
        }

        private IEnumerable<TModel> GetMany(IEnumerable<IntranetActivityEntity> entities)
        {
            foreach (var entity in entities)
            {
                var model = FillPropertiesOnGet(entity);
                yield return model;
            }
        }

        private static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }
    }
}