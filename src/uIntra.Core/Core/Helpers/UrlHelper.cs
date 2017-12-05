using System.Web;
using uIntra.Core.Extensions;

namespace uIntra.Core
{
    public static class UrlHelper
    {
        public static string GetFullUrl(this string url)
        {
            return url.IsNullOrEmpty() ? string.Empty : $"{GetHostUrl()}/{url}";
        }

        private static string GetHostUrl()
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}";
        }
    }
}