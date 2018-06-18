using System;
using System.Web;
using System.Web.Mvc;
using uIntra.Core.Constants;
using uIntra.Core.User;

namespace uIntra.Core.Extensions
{
    public static class HttpContextExtensions
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

        public static T GetCurrentUser<T>(this HttpContext context) where T : class, IIntranetUser
        {
            var currentUser = context.Session?[IntranetConstants.Session.LoggedUserSessionKey] as T;

            if (currentUser == null)
            {
                throw new Exception("Can't get current user from session!");
            }

            return currentUser;
        }
    }
}