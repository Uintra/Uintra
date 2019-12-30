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

        private readonly IDocumentTypeAliasProvider _aliasProvider;
        private readonly string _feedActivitiesAlias;


		public CacheActivityPageHelperFactory(
        
            IDocumentTypeAliasProvider aliasProvider)
        {
	        _aliasProvider = aliasProvider;
            _feedActivitiesAlias = _aliasProvider.GetHomePage();
        }

        public IActivityPageHelper GetHelper(Enum type)
        {
			var feedActivitiesContent = Umbraco.Web.Composing.Current.UmbracoHelper.ContentAtRoot().First(x => x.ContentType.Alias == _feedActivitiesAlias);
			var cacheKey = GetCacheKey(type, feedActivitiesContent.ContentType.Alias);
            if (!_cache.ContainsKey(cacheKey))
                return _cache[cacheKey] = CreateNewHelper(type, feedActivitiesContent);
            return _cache[cacheKey];
        }

        private string GetCacheKey(Enum type, string alias) => $"{type.ToString()}{alias}";

        private IActivityPageHelper CreateNewHelper(Enum type, IPublishedContent feedActivitiesContent)
        {
            switch (type)
            {
                default:
                    return new ActivityPageHelper(type, feedActivitiesContent, _aliasProvider);
            }
        }
    }
}