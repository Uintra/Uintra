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
            PermissionSettingSchema permissionSettingHierarchicalItem,
            PermissionSettingValues settingValues)
        {
            Group = group;
            SettingIdentity = permissionSettingHierarchicalItem.SettingIdentity;
            ParentActionType = permissionSettingHierarchicalItem.ParentActionType;
            SettingValues = settingValues;
        }
    }
}