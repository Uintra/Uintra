namespace Uintra20.Features.Permissions.Models
{
    public class PermissionUpdateModel
    {
        public IntranetMemberGroup Group { get; }
        public PermissionSettingIdentity SettingIdentity { get; }
        public PermissionSettingValues SettingValues { get; }

        public PermissionUpdateModel(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            PermissionSettingIdentity settingIdentity)
        {
            Group = group;
            SettingIdentity = settingIdentity;
            SettingValues = settingValues;
        }
    }
}