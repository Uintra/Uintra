using System;

namespace Uintra20.Features.Permissions.Models
{
    public class PermissionManagementModel
    {
        public IntranetMemberGroup Group { get; }
        public PermissionSettingValues SettingValues { get; }
        public Enum ParentActionType { get; }
        public PermissionSettingIdentity SettingIdentity { get; }

        public PermissionManagementModel(
            IntranetMemberGroup group,
            PermissionSettingIdentity settingIdentity,
            Enum parentActionType,
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