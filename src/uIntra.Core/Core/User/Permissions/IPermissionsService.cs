using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.User.Permissions
{
    public interface IPermissionsService
    {
        bool IsRoleHasPermissions(IRole role, params string[] permissions);
        IEnumerable<string> GetRolePermission(IRole role);
        string GetPermissionFromTypeAndAction(IIntranetType activityType, IntranetActivityActionEnum action);

        bool IsCurrentUserHasAccess(IIntranetType activityType, IntranetActivityActionEnum action);
        bool IsUserHasAccess(IIntranetUser user, IIntranetType activityType, IntranetActivityActionEnum action);
        bool IsUserWebmaster(IIntranetUser user);
    }
}