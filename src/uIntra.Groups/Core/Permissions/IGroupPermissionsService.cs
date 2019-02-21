using Uintra.Core.Permissions;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public interface IGroupPermissionsService
    {
        bool ValidatePermission(IPublishedContent content);
        bool HasPermission(PermissionActionEnum action, PermissionActivityTypeEnum activityType);
    }
}