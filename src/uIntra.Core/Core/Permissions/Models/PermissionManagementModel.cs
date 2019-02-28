using System;
using LanguageExt;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionManagementModel
    {
        public IntranetMemberGroup Group { get; }
        public PermissionSettingValues SettingValues { get; }
        public Option<Enum> ParentActionType { get; }
        public PermissionSettingIdentity SettingIdentity { get; }

        public PermissionManagementModel(
            IntranetMemberGroup group,
            PermissionSettingIdentity settingIdentity,
            Option<Enum> parentActionType,
            PermissionSettingValues settingValues)
        {
            Group = group;
            SettingIdentity = settingIdentity;
            ParentActionType = parentActionType;
            SettingValues = settingValues;
        }

        public static PermissionManagementModel Of(
            IntranetMemberGroup group,
            PermissionSettingSchema permissionSettingHierarchicalItem,
            PermissionSettingValues settingValues) =>
            new PermissionManagementModel(
                group,
                permissionSettingHierarchicalItem.SettingIdentity,
                permissionSettingHierarchicalItem.ParentActionType,
                settingValues);
    }
}
