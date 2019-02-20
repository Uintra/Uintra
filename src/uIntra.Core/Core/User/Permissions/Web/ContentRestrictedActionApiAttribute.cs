using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;

namespace Uintra.Core.User.Permissions.Web
{
    public class ContentRestrictedActionApiAttribute : ActionFilterAttribute
    {
        private readonly PermissionActivityTypeEnum _activityType;
        private readonly PermissionActionEnum _action;

        public ContentRestrictedActionApiAttribute(PermissionActivityTypeEnum activityType, PermissionActionEnum action)
        {
            _activityType = activityType;
            _action = action;
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IBasePermissionsService>();            

            var isMemberHasAccess = permissionsService.Check(_activityType, _action);

            if (!isMemberHasAccess)
            {
                Deny(filterContext);
            }
        }

        private void Deny(HttpActionContext filterContext)
        {
            filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User doesn't have permission for this action !");
        }
    }
}
