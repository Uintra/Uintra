using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Extensions;
using Uintra.Core.Constants;
using Uintra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra.Core.Extensions
{
    public static class PublishedContentExtensions
    {
        private const string UmbracoExtensionPropertyAlias = "umbracoExtension";

        public static IPublishedContent FirstChild(this IPublishedContent parent, string childAlias)
        {
            return parent.FirstChild(s => s.DocumentTypeAlias == childAlias);
        }

        public static string GetMediaExtension(this IPublishedContent content)
        {
            return content.GetPropertyValue<string>(UmbracoExtensionPropertyAlias, default(string));
        }

        public static string GetFileName(this IPublishedContent content)
        {
            var fullName = content.GetPropertyValue<string>(UmbracoAliases.Media.UmbracoFilePropertyAlias, content.Name);
            var result = ParseFullFileName(fullName);
            return result;
        }

        private static string ParseFullFileName(string fullName)
        {
            string regex = @"^/media/[0-9]*/(.*)$";
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
            return content.GetPropertyValue<Guid?>(IntranetConstants.IntranetCreatorId, null);
        }

        public static Guid? GetIntranetUserId(this IMedia content)
        {
            return content.GetValue<Guid?>(IntranetConstants.IntranetCreatorId);
        }

        public static Enum GetMediaType(this IPublishedContent content)
        {       
            switch (content.DocumentTypeAlias)
            {
                case UmbracoAliases.Media.ImageTypeAlias:
                    return MediaTypeEnum.Image;
                case UmbracoAliases.Media.FileTypeAlias:
                    return MediaTypeEnum.Document;
                case UmbracoAliases.Media.VideoTypeAlias:
                    return MediaTypeEnum.Video;
                default:
                    throw new Exception($"undefined document type - {content.DocumentTypeAlias}");

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
            if (key.HasValue() && value != null)
            {
                var keyValue = $"{key}={value}";
                result = $"{result.TrimEnd('/')}?{keyValue}";
            }

            return result;
        }

        public static string UrlWithQueryString(this string url, string key, object value)
        {
            var result = url;
            if (key.HasValue() && value != null)
            {
                var keyValue = $"{key}={value}";
                result = $"{result.TrimEnd('/')}?{keyValue}";
            }

            return result;
        }

        public static Guid GetGuidKey(this IPublishedContent content)
        {
            Guid result;
            switch (content)
            {
                case IPublishedContentWithKey contentWithKey:
                    result = contentWithKey.Key;
                    break;
                case PublishedContentWrapped wrapped when wrapped.Unwrap() is IPublishedContentWithKey contentWithKey:
                    result = contentWithKey.Key;
                    break;
                default:
                    result = default;
                    break;
            }
            return result;
        }
    }
}