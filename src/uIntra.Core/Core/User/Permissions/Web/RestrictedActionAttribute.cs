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
        private readonly bool _childAction;

        public RestrictedActionAttribute(PermissionResourceTypeEnum resourceType, PermissionActionEnum action, bool childAction = false)
        {
            _permissionSettingIdentity = PermissionSettingIdentity.Of(action, resourceType);
            _childAction = childAction;
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
                Deny(context, filterContext);
            }
        }

        private void Deny(HttpContextBase context, ActionExecutingContext filterContext)
        {
            if (_childAction)
            {
                filterContext.Result = new EmptyResult();
            }
            else
            {
                context.Response.StatusCode = HttpStatusCode.Forbidden.ToInt();
                context.ApplicationInstance.CompleteRequest();
            }
        }

        private static bool Skip(ActionExecutingContext context)
        {
            return context.ActionDescriptor.GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false).Any() ||
                   context.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false).Any();
        }
    }
}