using System.Web;
using System.Web.Mvc;

namespace uCommunity.Core.Extentions
{
    public static class HttpContextExtentions
    {
        public static T GetService<T>(this HttpContext context)
            where T : class
        {
            var key = typeof(T).FullName;

            var service = context.Items[key] as T;

            if (service == null)
            {
                context.Items[key] = service = DependencyResolver.Current.GetService<T>();
            }

            return service;
        }

        public static string GetBackLink(this HttpContextBase context, string defaultLink)
        {
            return context.Request.UrlReferrer?.AbsoluteUri ?? defaultLink;
        }
    }
}