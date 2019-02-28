using Uintra.Core.Permissions.Models;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public interface IGroupPermissionsService
    {
        bool ValidatePermission(IPublishedContent content);
        bool HasPermission(PermissionSettingIdentity permissionSettingIdentity);
    }
}