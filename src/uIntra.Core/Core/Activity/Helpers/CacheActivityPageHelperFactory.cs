using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using Umbraco.Web;

namespace uIntra.Core.Activity
{
    public class CacheActivityPageHelperFactory : IActivityPageHelperFactory
    {
        private readonly Dictionary<int, IActivityPageHelper> _cache = new Dictionary<int, IActivityPageHelper>();

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CacheActivityPageHelperFactory(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider aliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = aliasProvider;
        }

        public IActivityPageHelper GetHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            if (!_cache.ContainsKey(type.Id))
                return _cache[type.Id] = CreateNewHelper(type, baseXPath);
            return _cache[type.Id];
        }

        private IActivityPageHelper CreateNewHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            return new ActivityPageHelper(type, baseXPath, _umbracoHelper, _aliasProvider);
        }
    }
}