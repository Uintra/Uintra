using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.IO;

namespace uIntra.Core.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private static List<string> _reservedPaths;

        private static List<string> ReservedPaths
        {
            get
            {
                if (_reservedPaths != null) return _reservedPaths;

                var umbracoReservedPaths = ConfigurationManager.AppSettings["umbracoReservedPaths"] ?? string.Empty;

                _reservedPaths = umbracoReservedPaths
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(path => !string.IsNullOrWhiteSpace(path))
                    .Select(path => IOHelper.ResolveUrl(path.Trim()).Trim().ToLower())
                    .Where(path => path.Length > 0)
                    .Select(path => path + (path.EndsWith("/") ? string.Empty : "/"))
                    .ToList();

                return _reservedPaths;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url != null && IsUriAllowed(filterContext.HttpContext.Request.Url)) return;
            base.OnAuthorization(filterContext);
        }

        private static bool IsUriAllowed(Uri uri)
        {
            var pathPart = uri.AbsolutePath.Split('?')[0];
            if (!pathPart.Contains(".") && !pathPart.EndsWith("/"))
            {
                pathPart += "/";
            }

            return ReservedPaths.Exists(path => path.Equals(pathPart.ToLowerInvariant()));
        }
    }
}
