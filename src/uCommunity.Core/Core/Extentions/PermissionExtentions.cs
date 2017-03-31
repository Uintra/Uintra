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
    }
}