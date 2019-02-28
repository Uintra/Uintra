using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.User.Permissions.Web
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly PermissionSettingIdentity _permissionSettingIdentity;

        public RestrictedActionAttribute(PermissionResourceTypeEnum resourceType, PermissionActionEnum actionType)
        {
            _permissionSettingIdentity = PermissionSettingIdentity.Of(actionType, resourceType);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (Skip(filterContext))
            {
                return;
            }

            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();            
            var isUserHasAccess = permissionsService.Check(_permissionSettingIdentity);

            if (!isUserHasAccess)
            {
                var context = filterContext.Controller.ControllerContext.HttpContext;
                Deny(context);
            }
        }

        private void Deny(HttpContextBase context)
        {
            context.Response.StatusCode = HttpStatusCode.Forbidden.GetHashCode();
            context.Response.End();
        }

        private static bool Skip(ActionExecutingContext context)
        {
            return context.ActionDescriptor.GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false).Any() ||
                   context.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false).Any();
        }
    }
}