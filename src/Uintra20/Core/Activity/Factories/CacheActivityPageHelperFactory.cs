using System;
using System.Collections.Generic;
using Compent.Extensions;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Web;

namespace Uintra20.Core.Activity.Factories
{
    public class CacheActivityPageHelperFactory : IActivityPageHelperFactory
    {
        private readonly Dictionary<string, IActivityPageHelper> _cache = new Dictionary<string, IActivityPageHelper>();
        private readonly IEnumerable<string> _feedActivitiesXPath;

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CacheActivityPageHelperFactory(
            UmbracoHelper umbracoHelper,
            IDocumentTypeAliasProvider aliasProvider,
            IEnumerable<string> feedActivitiesXPath)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = aliasProvider;
            _feedActivitiesXPath = feedActivitiesXPath;
        }

        public IActivityPageHelper GetHelper(Enum type)
        {
            var xPath = _feedActivitiesXPath.AsList();
            var cacheKey = GetCacheKey(type, xPath);
            if (!_cache.ContainsKey(cacheKey))
                return _cache[cacheKey] = CreateNewHelper(type, xPath);
            return _cache[cacheKey];
        }

        private string GetCacheKey(Enum type, IEnumerable<string> xPath) => $"{type.ToString()}{xPath.JoinToString("")}";

        private IActivityPageHelper CreateNewHelper(Enum type, IEnumerable<string> baseXPath)
        {
            switch (type)
            {
                default:
                    return new ActivityPageHelper(type, baseXPath, _umbracoHelper, _aliasProvider);
            }
        }
    }
}