using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.PagePromotion
{
    public static class PagePromotionHelper
    {
        public static bool IsPagePromotion(IContent content) => content.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

        public static bool IsPagePromotion(IPublishedContent publishedContent) => publishedContent.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

        public static PagePromotionConfig GetConfig(IPublishedContent publishedContent)
        {
            var prop = publishedContent.GetPropertyValue<string>(PagePromotionConstants.PagePromotionConfigAlias);
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

            return config == null ?
                Enumerable.Empty<int>() :
                config.Files.ToIntCollection();
        }

        public static bool IsPromoted(IPublishedContent publishedContent)
        {
            if (!IsPagePromotion(publishedContent)) return false;

            var config = GetConfig(publishedContent);
            return config != null && config.PromoteOnCentralFeed;
        }

        private static PagePromotionConfig GetConfig(string propertyValue)
        {
            return propertyValue.IsNullOrEmpty() ? null : propertyValue.Deserialize<PagePromotionConfig>();
        }
    }
}