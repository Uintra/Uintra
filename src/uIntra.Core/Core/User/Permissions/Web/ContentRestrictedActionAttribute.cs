using System;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;

namespace Uintra.Core.User.Permissions.Web
{
    public class ContentRestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly Enum _activityType;
        private readonly IntranetActivityActionEnum _action;

        public ContentRestrictedActionAttribute(Enum activityType, IntranetActivityActionEnum action)
        {
            _activityType = activityType;
            _action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var isUserHasAccess = permissionsService.IsCurrentMemberHasAccess(_activityType, _action);

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
