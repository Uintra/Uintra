using System.Collections.Generic;
using uCommunity.Core.Activity;

namespace uCommunity.Core.User.Permissions
{
    public interface IPermissionsService
    {
        bool IsRoleHasPermissions(IntranetRolesEnum role, params string[] permissions);
        IEnumerable<string> GetRolePermission(IntranetRolesEnum role);
        string GetPermissionFromTypeAndAction(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action);
    }
}