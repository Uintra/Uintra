using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Permissions.Models;

namespace Uintra20.Features.Permissions.Interfaces
{
    public interface IPermissionSettingsSchemaProvider
    {
        PermissionSettingValues DefaultSettingsValues { get; }
        PermissionSettingSchema[] Settings { get; }
        PermissionSettingValues GetDefault(PermissionSettingIdentity settingIdentity);
        ILookup<PermissionSettingIdentity, PermissionSettingIdentity> SettingsByParentSettingIdentityLookup { get; }
        IEnumerable<PermissionSettingIdentity> GetDescendants(PermissionSettingIdentity parent);
    }
}
