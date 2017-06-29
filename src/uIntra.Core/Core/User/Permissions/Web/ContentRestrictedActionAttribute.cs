using System.Web;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;

namespace uIntra.Core.User.Permissions.Web
{
    public class ContentRestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly IActivityType _activityType;
        private readonly IntranetActivityActionEnum _action;

        public ContentRestrictedActionAttribute(IActivityType activityType, IntranetActivityActionEnum action)
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
                Deny(filterContext);
            }
        }

        private void Deny(ActionExecutingContext filterContext)
        {
            filterContext.Result = new EmptyResult();
        }
    }
}
