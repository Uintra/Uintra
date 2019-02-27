using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;

namespace Uintra.Core.User.Permissions.Web
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly PermissionActivityTypeEnum _activityType;
        private readonly PermissionActionEnum _action;
        private readonly bool _childAction;

        public RestrictedActionAttribute(PermissionActivityTypeEnum activityType, PermissionActionEnum action, bool childAction = false)
        {
            _activityType = activityType;
            _action = action;
            _childAction = childAction;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (Skip(filterContext))
            {
                return;
            }

            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();            
            var isUserHasAccess = permissionsService.Check(_activityType, _action);

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
                context.Response.StatusCode = HttpStatusCode.Forbidden.GetHashCode();
                context.Response.End();
            }
        }

        private static bool Skip(ActionExecutingContext context)
        {
            return context.ActionDescriptor.GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false).Any() ||
                   context.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false).Any();
        }
    }
}