using System.Net;
using System.Web;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.User.Permissions.Web
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly int _activityTypeId;
        private readonly IntranetActivityActionEnum _action;

        public RestrictedActionAttribute(int activityTypeId, IntranetActivityActionEnum action)
        {
            _activityTypeId = activityTypeId;
            _action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var provider = HttpContext.Current.GetService<IActivityTypeProvider>();
            var isUserHasAccess = permissionsService.IsCurrentUserHasAccess(provider.Get(_activityTypeId), _action);

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