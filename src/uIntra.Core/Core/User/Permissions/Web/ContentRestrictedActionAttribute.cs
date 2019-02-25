using System.Web;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;

namespace Uintra.Core.User.Permissions.Web
{
    public class ContentRestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly PermissionActivityTypeEnum _activityType;
        private readonly PermissionActionEnum _action;

        public ContentRestrictedActionAttribute(PermissionActivityTypeEnum activityType, PermissionActionEnum action)
        {
            _activityType = activityType;
            _action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var isUserHasAccess = permissionsService.Check(_activityType, _action);

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
