using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.User.Permissions.Web
{
    public class ContentRestrictedActionApiAttribute : ActionFilterAttribute
    {
        private readonly int _activityTypeId;
        private readonly IntranetActivityActionEnum _action;

        public ContentRestrictedActionApiAttribute(int activityTypeId, IntranetActivityActionEnum action)
        {
            _activityTypeId = activityTypeId;
            _action = action;
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IOldPermissionsService>();
            var activityTypeProvider = HttpContext.Current.GetService<IActivityTypeProvider>();

            var isMemberHasAccess = permissionsService.IsCurrentMemberHasAccess(activityTypeProvider[_activityTypeId], _action);

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
