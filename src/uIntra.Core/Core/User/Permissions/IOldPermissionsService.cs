using System;
using System.Collections.Generic;
using Uintra.Core.Activity;
using Umbraco.Core.Models;

namespace Uintra.Core.User.Permissions
{
    public interface IOldPermissionsService
    {
        bool IsRoleHasPermissions(IRole role, params string[] permissions);
        IEnumerable<string> GetRolePermission(IRole role);
        string GetPermissionFromTypeAndAction(Enum activityType, IntranetActionEnum action);

        bool IsCurrentMemberHasAccess(Enum activityType, IntranetActionEnum action, Guid? activityId = null);
        bool IsUserHasAccess(IIntranetMember member, Enum activityType, IntranetActionEnum action, Guid? activityId = null);
        bool IsUserWebmaster(IIntranetMember member);
        bool IsUserHasAccessToContent(IIntranetMember member, IPublishedContent content);
    }
}