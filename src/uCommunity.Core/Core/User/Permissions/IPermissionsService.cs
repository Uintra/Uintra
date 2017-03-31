using System.Collections.Generic;

namespace uCommunity.Core.User.Permissions
{
    public interface IPermissionsService
    {
        bool IsRoleHasPermissions(IntranetRolesEnum role, params string[] permissions);
        IEnumerable<string> GetRolePermission(IntranetRolesEnum role);
    }
}