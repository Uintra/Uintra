using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.User.Permissions.Web
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly int _activityTypeId;
        private readonly IntranetActivityActionEnum _action;
        private const string ActivityIdParameterName = "id";

        public RestrictedActionAttribute(int activityTypeId, IntranetActivityActionEnum action)
        {
            _activityTypeId = activityTypeId;
            _action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid? activityId;
            if (filterContext.ActionParameters.TryGetValue(ActivityIdParameterName, out var obj))
            {
                activityId = obj as Guid?;
            }
            else
            {
                activityId = null;
            }

            if (Skip(filterContext))
            {
                return;
            }

            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var provider = HttpContext.Current.GetService<IActivityTypeProvider>();
            var isUserHasAccess = permissionsService.IsCurrentUserHasAccess(provider.Get(_activityTypeId), _action, activityId);

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