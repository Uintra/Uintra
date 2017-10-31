using System;
using System.Linq;
using System.Web.Mvc;

namespace uIntra.Core.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string UmbracoIdentifier = "umbraco";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url != null && IsUriAllowed(filterContext.HttpContext.Request.Url)) return;
            base.OnAuthorization(filterContext);
        }

        private static bool IsUriAllowed(Uri uri) => uri.Segments.Any(s => s.Contains(UmbracoIdentifier));
    }
}
