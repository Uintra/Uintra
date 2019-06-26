
namespace Uintra.Core.Permissions.Models
{
    public class PermissionUpdateModel
    {
        public IntranetMemberGroup Group { get; }
        public PermissionSettingIdentity SettingIdentity { get; }
        public PermissionSettingValues SettingValues { get; }

        private PermissionUpdateModel(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            PermissionSettingIdentity settingIdentity)
        {
            Group = group;
            SettingIdentity = settingIdentity;
            SettingValues = settingValues;
        }

        public static PermissionUpdateModel Of(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            PermissionSettingIdentity identity) =>
            new PermissionUpdateModel(group, settingValues, identity);
    }
}
