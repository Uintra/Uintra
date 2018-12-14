using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using LanguageExt;
using Uintra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;
using static LanguageExt.Prelude;

namespace Uintra.Core.PagePromotion
{
    public static class PagePromotionHelper
    {
        public static bool IsPagePromotion(IContent content) => content.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

        public static bool IsPagePromotion(IPublishedContent publishedContent) =>
            publishedContent.HasProperty(PagePromotionConstants.PagePromotionConfigAlias);

        public static Option<PagePromotionConfig> GetConfig(IPublishedContent publishedContent)
        {
            var prop = publishedContent.GetPropertyValue<string>(PagePromotionConstants.PagePromotionConfigAlias);
            return GetConfig(prop);
        }

        public static bool IsPromoted(IPublishedContent publishedContent) =>
            IsPagePromotion(publishedContent) &&
            GetConfig(publishedContent)
                .Filter(cfg => !cfg.PromoteOnCentralFeed).IsSome;

        public static Option<PagePromotionConfig> GetConfig(IContent content) =>
            content
                .GetValue<string>(PagePromotionConstants.PagePromotionConfigAlias)
                .Apply(GetConfig);

        private static Option<PagePromotionConfig> GetConfig(string propertyValue) =>
            propertyValue.IsNullOrEmpty()
                ? None
                : Some(propertyValue.Deserialize<PagePromotionConfig>());
    }
}