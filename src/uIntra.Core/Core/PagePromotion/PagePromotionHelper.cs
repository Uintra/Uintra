using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.PagePromotion
{
    public class PagePromotionHelper
    {
        public static bool IsPagePromotion(IContent content) => content.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

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

        private static PagePromotionConfig GetConfig(string propertyValue)
        {
            return propertyValue.IsNullOrEmpty() ? null : propertyValue.Deserialize<PagePromotionConfig>();
        }
    }
}