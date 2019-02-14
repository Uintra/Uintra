using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IPermissionSettingsSchema
    {
        PermissionSettingIdentity[] Settings { get; }
        PermissionManagementModel GetDefault(PermissionSettingIdentity settingIdentity, IntranetMemberGroup group);
    }
}