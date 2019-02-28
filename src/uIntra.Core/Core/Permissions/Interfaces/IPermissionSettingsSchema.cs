using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IPermissionSettingsSchemaProvider
    {
        PermissionSettingSchema[] Settings { get; }
        PermissionSettingValues GetDefault(PermissionSettingIdentity settingIdentity);
    }
}