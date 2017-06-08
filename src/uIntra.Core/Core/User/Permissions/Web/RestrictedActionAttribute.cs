using System.Net;
using System.Web;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;

namespace uIntra.Core.User.Permissions.Web
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly IntranetActivityTypeEnum _activityType;
        private readonly IntranetActivityActionEnum _action;

        public RestrictedActionAttribute(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action)
        {
            _activityType = activityType;
            _action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var isUserHasAccess = permissionsService.IsCurrentUserHasAccess(_activityType, _action);

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
    }
}