using uIntra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.PagePromotion
{
    public class PagePromotionConfigHelper
    {
        public static PagePromotionConfig GetPagePromotionConfig(IPublishedContent content)
        {
            var prop = content.GetPropertyValue<string>(PagePromotionConstants.PagePromotionConfigAlias);
            if (prop.IsNullOrEmpty()) return null;

            return prop.Deserialize<PagePromotionConfig>();
        }
    }
}