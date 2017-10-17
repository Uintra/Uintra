using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;
using Umbraco.Web;

namespace uIntra.Core.Activity
{
    public class CacheActivityPageHelperFactory : IActivityPageHelperFactory
    {
        private readonly Dictionary<string, IActivityPageHelper> _cache = new Dictionary<string, IActivityPageHelper>();

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CacheActivityPageHelperFactory(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider aliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = aliasProvider;
        }

        public IActivityPageHelper GetHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            var xPath = baseXPath as string[] ?? baseXPath.ToArray();
            string cacheKey = GetCacheKey(type, xPath);
            if (!_cache.ContainsKey(cacheKey))
                return _cache[cacheKey] = CreateNewHelper(type, xPath);
            return _cache[cacheKey];
        }

        private string GetCacheKey(IIntranetType type, string[] xPath) => 
            type.Name + xPath.Aggregate((a, s) => a + s);

        private IActivityPageHelper CreateNewHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            return new ActivityPageHelper(type, baseXPath, _umbracoHelper, _aliasProvider);
        }
    }
}