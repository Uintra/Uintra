using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;
using Umbraco.Web;

namespace uIntra.Core.Activity
{
    public class CacheActivityPageHelperFactory : IActivityPageHelperFactory
    {
        private readonly Dictionary<string, IActivityPageHelper> _cache = new Dictionary<string, IActivityPageHelper>();

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;
        private readonly IPagePromotionService<PagePromotionBase> _pagePromotionService;

        public CacheActivityPageHelperFactory(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider aliasProvider, IPagePromotionService<PagePromotionBase> pagePromotionService)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = aliasProvider;
            _pagePromotionService = pagePromotionService;
        }

        public IActivityPageHelper GetHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            var xPath = baseXPath as string[] ?? baseXPath.ToArray();
            string cacheKey = GetCacheKey(type, xPath);
            if (!_cache.ContainsKey(cacheKey))
                return _cache[cacheKey] = CreateNewHelper(type, xPath);
            return _cache[cacheKey];
        }

        private string GetCacheKey(IIntranetType type, string[] xPath) => $"{type.Name}{xPath.JoinToString("")}";

        private IActivityPageHelper CreateNewHelper(IIntranetType type, IEnumerable<string> baseXPath)
        {
            switch (type.Id)
            {
                case (int)IntranetActivityTypeEnum.PagePromotion:
                    {
                        return new PagePromotionPageHelper(type, _pagePromotionService);
                    }

                default:
                    return new ActivityPageHelper(type, baseXPath, _umbracoHelper, _aliasProvider);
            }
        }
    }
}