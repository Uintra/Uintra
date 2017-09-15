using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using Umbraco.Web;

namespace uIntra.Core.Activity
{
    public class CacheActivityPageHelperFactory : IActivityPageHelperFactory
    {
        private readonly Dictionary<IIntranetType, IActivityPageHelper> _cache = new Dictionary<IIntranetType, IActivityPageHelper>();

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CacheActivityPageHelperFactory(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider aliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = aliasProvider;
        }

        public IActivityPageHelper GetHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            if (!_cache.ContainsKey(type))
                return _cache[type] = CreateNewHelper(type, baseXPath);
            return _cache[type];

        }

        private IActivityPageHelper CreateNewHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            return new ActivityPageHelper(type, baseXPath, _umbracoHelper, _aliasProvider);
        }
    }
}