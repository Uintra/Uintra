using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Core.Extentions
{
    public static class PublishedContentExtentions
    {
        public static bool GetHideInNavigation(this IPublishedContent content)
        {
            if (content is INavigationComposition)
            {
                var typedModel = (INavigationComposition)content;
                return typedModel.IsHideFromLeftNavigation;
            }

            return true;
        }

        public static bool IsShowPageInSubNavigation(this IPublishedContent content)
        {
            if (content is INavigationComposition)
            {
                var typedModel = (INavigationComposition)content;
                return !typedModel.IsHideFromSubNavigation;
            }

            return true;
        }

        public static string GetNavigationName(this IPublishedContent content)
        {
            if (content is INavigationComposition)
            {
                var typedModel = (INavigationComposition)content;
                return typedModel.NavigationName;
            }

            return string.Empty;
        }

    }
}