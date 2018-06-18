using System.Collections.Generic;
using System.Linq;
using Extensions;
using Uintra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.PagePromotion
{
    public static class PagePromotionHelper
    {
        public static bool IsPagePromotion(IContent content) => content.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

        public static bool IsPagePromotion(IPublishedContent content) => content.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

        public static PagePromotionConfig GetConfig(IPublishedContent content)
        {
            var prop = content.GetPropertyValue<string>(PagePromotionConstants.PagePromotionConfigAlias);
            return GetConfig(prop);
        }

        public static PagePromotionConfig GetConfig(IContent content)
        {
            var prop = content.GetValue<string>(PagePromotionConstants.PagePromotionConfigAlias);
            return GetConfig(prop);
        }

        public static IEnumerable<int> GetFileIds(IContent content)
        {
            var config = GetConfig(content);
            if (config == null) return Enumerable.Empty<int>();

            return config.Files.ToIntCollection();
        }

        public static bool IsPromoted(IPublishedContent content)
        {
            var config = GetConfig(content);
            return config != null && config.PromoteOnCentralFeed;
        }

        private static PagePromotionConfig GetConfig(string propertyValue)
        {
            return propertyValue.IsNullOrEmpty() ? null : propertyValue.Deserialize<PagePromotionConfig>();
        }
    }
}