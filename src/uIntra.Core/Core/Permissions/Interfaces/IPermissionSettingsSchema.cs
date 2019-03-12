using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
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