using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.Core.Extentions;
using Umbraco.Core;

namespace uCommunity.Core.User.Permissions
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private IntranetActivityTypeEnum? activityType;
        private readonly IntranetActivityActionEnum action;

        public RestrictedActionAttribute(IntranetActivityActionEnum action)
        {
            this.action = action;
        }

        public RestrictedActionAttribute(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action)
        {
            this.activityType = activityType;
            this.action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckActivityType(filterContext);

            var context = filterContext.Controller.ControllerContext.HttpContext;
            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var userService = HttpContext.Current.GetService<IIntranetUserService>();

            var currentUser = userService.GetCurrentUser();

            if (currentUser != null)
            {
                var permission = $"{activityType}{action}";
                var userHasPermissions = permissionsService.IsRoleHasPermissions(currentUser.Role, permission);
                if (userHasPermissions)
                {
                    return;
                }
            }
            Deny(context);
        }

        private void Deny(HttpContextBase context)
        {
            var urlToRedirect = context.Request.UrlReferrer?.AbsolutePath ?? "/";

            context.Response.StatusCode = HttpStatusCode.Unauthorized.GetHashCode();
            context.Response.Redirect(urlToRedirect, true);
        }

        private void CheckActivityType(ActionExecutingContext filterContext)
        {
            if (!activityType.HasValue)
            {
                var activityControllerAttribute = filterContext.Controller.GetType().GetCustomAttribute<ActivityControllerAttribute>(true);
                if (activityControllerAttribute != null)
                {
                    activityType = activityControllerAttribute.ActivityType;
                }
                else
                {
                    throw new ArgumentNullException($"ActivityType not defined in controller \"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}\"");
                }
            }
        }
    }
}