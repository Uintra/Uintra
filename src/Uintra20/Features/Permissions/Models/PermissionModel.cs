using System;

namespace Uintra20.Features.Permissions.Models
{
    public class PermissionModel
    {
        public Guid Id { get; }
        public IntranetMemberGroup Group { get; }
        public PermissionSettingValues SettingValues { get; }
        public PermissionSettingIdentity SettingIdentity { get; }

        public PermissionModel(
            Guid id, 
            PermissionSettingIdentity settingIdentity, 
            PermissionSettingValues settingValues, 
            IntranetMemberGroup group)
        {
            Id = id;
            Group = group;
            SettingIdentity = settingIdentity;
            SettingValues = settingValues;
        }
    }
}