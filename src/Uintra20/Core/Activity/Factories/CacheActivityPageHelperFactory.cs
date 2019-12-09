using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.Activity.Factories
{
    public class CacheActivityPageHelperFactory : IActivityPageHelperFactory
    {
        private readonly Dictionary<string, IActivityPageHelper> _cache = new Dictionary<string, IActivityPageHelper>();
        private readonly IPublishedContent _feedActivitiesContent;

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CacheActivityPageHelperFactory(
            UmbracoHelper umbracoHelper,
            IDocumentTypeAliasProvider aliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = aliasProvider;
            var feedActivitiesAlias = _aliasProvider.GetHomePage();
            _feedActivitiesContent = _umbracoHelper.ContentAtRoot().First(x => x.ContentType.Alias == feedActivitiesAlias);
        }

        public IActivityPageHelper GetHelper(Enum type)
        {
            var cacheKey = GetCacheKey(type, _feedActivitiesContent.ContentType.Alias);
            if (!_cache.ContainsKey(cacheKey))
                return _cache[cacheKey] = CreateNewHelper(type, _feedActivitiesContent);
            return _cache[cacheKey];
        }

        private string GetCacheKey(Enum type, string alias) => $"{type.ToString()}{alias}";

        private IActivityPageHelper CreateNewHelper(Enum type, IPublishedContent feedActivitiesContent)
        {
            switch (type)
            {
                default:
                    return new ActivityPageHelper(type, feedActivitiesContent, _umbracoHelper, _aliasProvider);
            }
        }
    }
}