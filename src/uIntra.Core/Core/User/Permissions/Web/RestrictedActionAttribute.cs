using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.Helpers;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;

namespace Uintra.Core.User.Permissions.Web
{
    public class RestrictedActionAttribute : ActionFilterAttribute
    {
        private readonly PermissionSettingIdentity _permissionSettingIdentity;
        private readonly bool _childAction;

        public RestrictedActionAttribute(int resourceType, int action, bool childAction = false)
        {
            var permissionActionTypeProvider = DependencyResolver.Current.GetService<IPermissionActionTypeProvider>();
            var permissionResourceTypeProvider = DependencyResolver.Current.GetService<IPermissionResourceTypeProvider>();

            _permissionSettingIdentity = PermissionSettingIdentity.Of(
                permissionActionTypeProvider[action],
                permissionResourceTypeProvider[resourceType]);

            _childAction = childAction;
        }

        public RestrictedActionAttribute(
            PermissionResourceTypeEnum resourceType,
            PermissionActionEnum action,
            bool childAction = false)
        {
            _permissionSettingIdentity = PermissionSettingIdentity.Of(action, resourceType);
            _childAction = childAction;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Skip(context))
            {
                return;
            }

            if (DoesNotHaveAccess())
            {
                Denied(context);
            }
        }

        private void Denied(ActionExecutingContext filterContext)
        {
            if (_childAction)
            {
                filterContext.Result = new EmptyResult();
            }
            else
            {
                TransferRequestHelper.ToForbiddenPage(filterContext);
            }
        }

        private static bool Skip(ActionExecutingContext context) => 
            HaveIgnoreRestrictedAttributes(context) || HaveIgnoreRestrictedDescriptorAttributes(context);

        private static bool HaveIgnoreRestrictedDescriptorAttributes(ActionExecutingContext context) =>
            context.ActionDescriptor
                .ControllerDescriptor
                .GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false)
                .Any();

        private static bool HaveIgnoreRestrictedAttributes(ActionExecutingContext context) =>
            context.ActionDescriptor
                .GetCustomAttributes(typeof(IgnoreRestrictedActionAttribute), false)
                .Any();

        private bool DoesNotHaveAccess() =>
            !HttpContext.Current
                .GetService<IPermissionsService>()
                .Check(_permissionSettingIdentity);
    }
}