using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.User.Permissions.Web
{
    public class ContentRestrictedActionApiAttribute : ActionFilterAttribute
    {
        private readonly PermissionSettingIdentity _permissionSettingIdentity;

        public ContentRestrictedActionApiAttribute(PermissionResourceTypeEnum resourceType, PermissionActionEnum action)
        {
            _permissionSettingIdentity = PermissionSettingIdentity.Of(action, resourceType);
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();            

            var isMemberHasAccess = permissionsService.Check(_permissionSettingIdentity);

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
