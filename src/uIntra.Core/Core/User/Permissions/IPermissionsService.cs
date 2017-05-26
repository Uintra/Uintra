using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.Core.User.Permissions
{
    public interface IPermissionsService
    {
        bool IsRoleHasPermissions<T>(T role, params string[] permissions) where T : struct;
        IEnumerable<string> GetRolePermission<T>(T role) where T : struct;
        string GetPermissionFromTypeAndAction(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action);
    }
}