using System.Web;
using Compent.uIntra.Core.Constants;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.Extentions
{
    public static class PublishedContentExtentions
    {
        public static bool GetHideInNavigation(this IPublishedContent content)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            if (content.IsComposedOf(docTypeAliasProvider.GetNavigationComposition()))
            {
                var isHideFromLeftNavigationPropName = content.GetPropertyValue<bool>(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName);

                return isHideFromLeftNavigationPropName;
            }

            return true;
        }

        public static bool IsShowPageInSubNavigation(this IPublishedContent content)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();
            
            if (content.IsComposedOf(docTypeAliasProvider.GetNavigationComposition()))
            {
                var isHideFromSubNavigation = content.GetPropertyValue<bool>(NavigationPropertiesConstants.IsHideFromSubNavigationPropName);

                return !isHideFromSubNavigation;
            }

            return true;
        }

        public static string GetNavigationName(this IPublishedContent content)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();
            if (content.IsComposedOf(docTypeAliasProvider.GetNavigationComposition()))
            {
                var navigationName = content.GetPropertyValue<string>(NavigationPropertiesConstants.NavigationNamePropName);
                
                return navigationName;
            }

            return string.Empty;
        }
    }
}