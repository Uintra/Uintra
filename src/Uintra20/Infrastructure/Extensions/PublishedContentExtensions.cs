using System;
using System.Text.RegularExpressions;
using System.Web;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Navigation;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Infrastructure.Extensions
{
    public static class PublishedContentExtensions
    {
        private const string UmbracoExtensionPropertyAlias = "umbracoExtension";

        public static IPublishedContent FirstChild(this IPublishedContent parent, string childAlias)
        {
            return parent.FirstChild(s => s.ContentType.Alias == childAlias);
        }

        public static string GetMediaExtension(this IPublishedContent content)
        {
            return content.Value<string>(UmbracoExtensionPropertyAlias, default(string));
        }

        public static string GetFileName(this IPublishedContent content)
        {
            var fullName = content.Value<string>(UmbracoAliases.Media.UmbracoFilePropertyAlias, content.Name);
            var result = ParseFullFileName(fullName);
            return result;
        }

        private static string ParseFullFileName(string fullName)
        {
            const string regex = @"^/media/[0-9]*/(.*)$";
            var match = Regex.Match(fullName, regex);
            try
            {
                var result = match.Groups[1].Value;
                return result;
            }
            catch (Exception) // if somebody [upload file with potentially invalid name]/[break umbraco structure], we will have global crash
            {
                return fullName;
            }
        }

        public static Guid? GetIntranetUserId(this IPublishedContent content)
        {
            return content.Value<Guid?>(IntranetConstants.IntranetCreatorId, null);
        }

        public static Guid? GetIntranetUserId(this IMedia content)
        {
            return content.GetValue<Guid?>(IntranetConstants.IntranetCreatorId);
        }

        public static Enum GetMediaType(this IPublishedContent content)
        {
            switch (content.ContentType.Alias)
            {
                case UmbracoAliases.Media.ImageTypeAlias:
                    return MediaTypeEnum.Image;
                case UmbracoAliases.Media.FileTypeAlias:
                    return MediaTypeEnum.Document;
                case UmbracoAliases.Media.VideoTypeAlias:
                    return MediaTypeEnum.Video;
                default:
                    throw new Exception($"undefined document type - {content.ContentType.Alias}");

            }
        }

        public static bool GetHideInNavigation(this IPublishedContent content)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            if (content.IsComposedOf(docTypeAliasProvider.GetNavigationComposition()))
            {
                var isHideFromLeftNavigationPropName = content.Value<bool>(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName);

                return isHideFromLeftNavigationPropName;
            }

            return true;
        }

        public static bool IsShowPageInSubNavigation(this IPublishedContent content)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            if (content.IsComposedOf(docTypeAliasProvider.GetNavigationComposition()))
            {
                var isHideFromSubNavigation = content.Value<bool>(NavigationPropertiesConstants.IsHideFromSubNavigationPropName);

                return !isHideFromSubNavigation;
            }

            return true;
        }

        public static string GetNavigationName(this IPublishedContent content)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();
            if (content.IsComposedOf(docTypeAliasProvider.GetNavigationComposition()))
            {
                var navigationName = content.Value<string>(NavigationPropertiesConstants.NavigationNamePropName);

                return navigationName;
            }

            return string.Empty;
        }

        public static bool IsHeading(this IPublishedContent publishedContent)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();
            return publishedContent.ContentType.Alias.Equals(docTypeAliasProvider.GetHeading());
        }

        public static bool IsContentPage(this IPublishedContent publishedContent)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();
            return publishedContent.ContentType.Alias.Equals(docTypeAliasProvider.GetArticlePage());
        }
    }
}