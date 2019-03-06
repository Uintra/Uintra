using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IPermissionSettingsSchemaProvider
    {
        PermissionSettingValues DefaultSettingsValues { get; }
        PermissionSettingSchema[] Settings { get; }
        PermissionSettingValues GetDefault(PermissionSettingIdentity settingIdentity);
    }
}