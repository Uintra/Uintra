using System;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionModel
    {
        public Guid Id { get; }
        public IntranetMemberGroup Group { get; }
        public PermissionSettingValues SettingValues { get; }
        public PermissionSettingIdentity SettingIdentity { get; }

        private PermissionModel(Guid id, IntranetMemberGroup group, PermissionSettingIdentity settingIdentity, PermissionSettingValues settingValues)
        {
            Id = id;
            Group = group;
            SettingIdentity = settingIdentity;
            SettingValues = settingValues;
        }

        public static PermissionModel Of(PermissionSettingIdentity settingIdentity, PermissionSettingValues settingValues,
            IntranetMemberGroup group) =>
            new PermissionModel(Guid.NewGuid(), group, settingIdentity, settingValues);
    }
}