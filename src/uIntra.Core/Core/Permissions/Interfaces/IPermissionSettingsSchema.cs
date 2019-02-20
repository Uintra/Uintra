using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions
{
    public interface IPermissionSettingsSchema
    {
        PermissionSettingIdentity[] Settings { get; }
        BasePermissionModel GetDefault(PermissionSettingIdentity settingIdentity, IntranetMemberGroup group);
    }
}