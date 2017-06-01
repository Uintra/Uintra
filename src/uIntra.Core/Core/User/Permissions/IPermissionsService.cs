using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.Core.User.Permissions
{
    public interface IPermissionsService
    {
        bool IsCurrentUserHasAccess(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action);

        bool IsRoleHasPermissions(IRole role, params string[] permissions);
        IEnumerable<string> GetRolePermission(IRole role);
        string GetPermissionFromTypeAndAction(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action);
    }
}