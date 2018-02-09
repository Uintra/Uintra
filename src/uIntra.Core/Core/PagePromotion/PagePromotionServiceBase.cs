using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.PagePromotion
{
    public abstract class PagePromotionServiceBase<T> : IPagePromotionService<T> where T : PagePromotionBase
    {
        private const string CacheKey = "PagePromotionCache";

        private readonly ICacheService _cacheService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        protected PagePromotionServiceBase(ICacheService cacheService, UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _cacheService = cacheService;
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public abstract Enum ActivityType { get; }

        public virtual void Delete(Guid id)
        {
            UpdateCachedEntity(id);
        }

        public virtual bool CanEdit(IIntranetActivity cached) => false;
        public virtual bool CanEdit(Guid id) => false;

        public virtual T Get(Guid id)
        {
            var cached = GetAll(true).SingleOrDefault(s => s.Id == id);
            return cached;
        }

        public virtual IEnumerable<T> GetManyActual()
        {
            var cached = GetAll(true);
            return cached.Where(IsActual);
        }

        public virtual IEnumerable<T> GetAll(bool includeHidden = false)
        {
            if (!_cacheService.HasValue(CacheKey))
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

        public virtual bool IsActual(IIntranetActivity cachedActivity)
        {
            var pagePromotion = cachedActivity as T;
            return pagePromotion != null && !pagePromotion.IsHidden && pagePromotion.PublishDate <= DateTime.Now;
        }

        public bool IsPinActual(IIntranetActivity cachedActivity) => false;
        public virtual Guid Create(IIntranetActivity activity) => activity.Id;

        public virtual void Save(IIntranetActivity activity)
        {
            UpdateCachedEntity(activity.Id);
        }

        protected virtual T UpdateCachedEntity(Guid id)
        {
            var activity = GetFromStorage(id);
            var cached = GetAll(true);
            var cachedList = (cached as List<T> ?? cached.ToList()).FindAll(s => s.Id != id);

            if (activity != null)
            {
                MapBeforeCache(activity.ToListOfOne());
                cachedList.Add(activity);
            }

            _cacheService.Set(CacheKey, cachedList, CacheHelper.GetMidnightUtcDateTimeOffset());
            return activity;
        }

        protected virtual void UpdateCache()
        {
            FillCache();
        }

        protected virtual void FillCache()
        {
            var items = GetAllFromStorage();
            _cacheService.Set(CacheKey, items, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        protected virtual IEnumerable<T> GetAllFromCache()
        {
            var activities = _cacheService.Get<IList<T>>(CacheKey);
            return activities;
        }

        protected virtual T GetFromStorage(Guid id)
        {
            var content = _umbracoHelper.TypedContent(id);
            if (content == null || !PagePromotionHelper.IsPagePromotion(content)) return null;

            return MapInternal(content);
        }

        protected virtual IList<T> GetAllFromStorage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(pc => pc.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()));

            var activities = homePage
                .Descendants()
                .Where(PagePromotionHelper.IsPagePromotion)
                .Select(MapInternal)
                .Where(pp => pp != null)
                .ToList();

            return activities;
        }

        protected abstract T MapInternal(IPublishedContent content);
        protected abstract void MapBeforeCache(IList<T> cached);

        protected virtual bool IsPagePromotionHidden(T pagePromotion) => pagePromotion == null || pagePromotion.IsHidden;
    }
}
