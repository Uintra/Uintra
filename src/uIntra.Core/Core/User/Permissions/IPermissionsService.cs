using System;
using System.Collections.Generic;
using uIntra.Core.Activity;
using Umbraco.Core.Models;

namespace uIntra.Core.User.Permissions
{
    public interface IPermissionsService
    {
        bool IsRoleHasPermissions(IRole role, params string[] permissions);
        IEnumerable<string> GetRolePermission(IRole role);
        string GetPermissionFromTypeAndAction(Enum activityType, IntranetActivityActionEnum action);

        bool IsCurrentUserHasAccess(Enum activityType, IntranetActivityActionEnum action, Guid? activityId = null);
        bool IsUserHasAccess(IIntranetUser user, Enum activityType, IntranetActivityActionEnum action, Guid? activityId = null);
        bool IsUserWebmaster(IIntranetUser user);
        bool IsUserHasAccessToContent(IIntranetUser user, IPublishedContent content);
    }
}