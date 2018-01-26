using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core;

namespace uIntra.Core.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private static readonly string[] UmbracoIdentifier = { "install", "umbraco" };

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url != null && IsUriAllowed(filterContext.HttpContext.Request.Url)) return;
            base.OnAuthorization(filterContext);
        }

        private static bool IsUriAllowed(Uri uri) => uri.Segments.Any(s => s.ContainsAny(UmbracoIdentifier));
    }
}
