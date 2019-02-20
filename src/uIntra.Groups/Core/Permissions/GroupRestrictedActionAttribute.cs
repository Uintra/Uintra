using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
using Uintra.Core.User;
using Uintra.Core.User.Permissions.Web;

namespace Uintra.Groups.Permissions
{
    public class GroupRestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly PermissionActionEnum _action;


        public GroupRestrictedActionAttribute(PermissionActionEnum action)
        {            
            _action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Skip(filterContext))
            {
                return;
            }

            var permissionsService = HttpContext.Current.GetService<IGroupPermissionsService>();            
            var isUserHasAccess = permissionsService.HasPermission(_action, PermissionActivityTypeEnum.Group);

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