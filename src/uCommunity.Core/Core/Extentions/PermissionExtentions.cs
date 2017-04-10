using System.Web;
using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;

namespace uCommunity.Core.Extentions
{
    public static class PermissionsExtentions
    {
        public static bool IsRoleHasPermissions(this IPermissionsService permissionsService, IIntranetUser user, params string[] permissions)
        {
            return permissionsService.IsRoleHasPermissions(user.Role, permissions);
        }

        public static bool IsRoleHasPermissions(this IPermissionsService permissionsService, IIntranetUser user, IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action)
        {
            return permissionsService.IsRoleHasPermissions(user.Role, GetPermission(activityType, action));
        }
        
        public static bool IsCurrentUserHasPermission(this HtmlHelper html, params string[] permissions)
        {
            var userService = HttpContext.Current.GetService<IIntranetUserService>();
            var currentUser = userService.GetCurrentUser();

            if (currentUser == null)
            {
                return false;
            }

            var permissionsService = HttpContext.Current.GetService<IPermissionsService>();
            var isAllowed = permissionsService.IsRoleHasPermissions(currentUser, permissions);
            return isAllowed;
        }

        public static bool IsCurrentUserHasPermission(this HtmlHelper html, IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action)
        {
            return IsCurrentUserHasPermission(html, GetPermission(activityType, action));
        }


        private static string GetPermission(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action)
        {
            var permission = $"{activityType}{action}";
            return permission;
        }
    }
}