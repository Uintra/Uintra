using System;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Core.App_Plugins.Core.Extentions
{
    public static class PublishedContentExtentions
    {
        private const string UmbracoExtensionPropertyAlias = "umbracoExtension";

        public static IPublishedContent FirstChild(this IPublishedContent parent, string childAlias)
        {
            return parent.FirstChild(s => s.DocumentTypeAlias == childAlias);
        }

        public static string GetMediaExtention(this IPublishedContent content)
        {
            return content.GetPropertyValue<string>(UmbracoExtensionPropertyAlias, default(string));
        }

        public static MediaTypeEnum GetMediaType(this IPublishedContent content)
        {
            switch (content.DocumentTypeAlias)
            {
                case UmbracoAliases.Media.ImageTypeAlias:
                    return MediaTypeEnum.Image;
                case UmbracoAliases.Media.FileTypeAlias:
                    return MediaTypeEnum.Document;
                default:
                    throw new NotImplementedException();

            }
        }

        public static string UrlWithParams(this IPublishedContent content, params string[] urlParams)
        {
            var result = content.Url;

            if (urlParams.Any())
            {
                result = $"{result.TrimEnd('/')}/{urlParams.JoinWithSeparator("/").Trim('/')}";
            }

            return result;
        }

        public static string UrlWithQueryString(this IPublishedContent content, string key, object value)
        {
            var result = content.Url;
            if (key.IsNotNullOrEmpty() && value != null)
            {
                var keyValue = $"{key}={value}";
                result = $"{result.TrimEnd('/')}?{keyValue}";
            }

            return result;
        }

        public static string UrlWithQueryString(this string url, string key, object value)
        {
            var result = url;
            if (key.IsNotNullOrEmpty() && value != null)
            {
                var keyValue = $"{key}={value}";
                result = $"{result.TrimEnd('/')}?{keyValue}";
            }

            return result;
        }
    }
}