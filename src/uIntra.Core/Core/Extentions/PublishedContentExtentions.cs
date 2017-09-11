using System;
using System.Linq;
using System.Web;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.Extentions
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

        public static Guid? GetIntranetUserId(this IPublishedContent content)
        {
            return content.GetPropertyValue<Guid?>(IntranetConstants.IntranetCreatorId, null);
        }

        public static Guid? GetIntranetUserId(this IMedia content)
        {
            return content.GetValue<Guid?>(IntranetConstants.IntranetCreatorId);
        }

        public static IIntranetType GetMediaType(this IPublishedContent content)
        {
            var mediaTypeProvider = HttpContext.Current.GetService<IMediaTypeProvider>();

            switch (content.DocumentTypeAlias)
            {
                case UmbracoAliases.Media.ImageTypeAlias:
                    return mediaTypeProvider.Get(MediaTypeEnum.Image.ToInt());
                case UmbracoAliases.Media.FileTypeAlias:
                    return mediaTypeProvider.Get(MediaTypeEnum.Document.ToInt());
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