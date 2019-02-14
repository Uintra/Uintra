using LanguageExt;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionManagementModel
    {
        public PermissionSettingIdentity SettingIdentity { get; }
        public PermissionSettingValues SettingValues { get; }
        public IntranetMemberGroup Group { get; }


        private PermissionManagementModel(
            PermissionSettingIdentity settingsIdentity,
            PermissionSettingValues settingValues,
            IntranetMemberGroup group)
        {
            SettingIdentity = settingsIdentity;
            SettingValues = settingValues;
            Group = group;
        }

        public static PermissionManagementModel Of(
            BasePermissionModel basePermission,
            Option<ActivityTypePermissionModel> activityTypePermission,
            IntranetMemberGroup group) =>
            new PermissionManagementModel(
                PermissionSettingIdentity.Of(basePermission.ActionType, activityTypePermission.Map(perm => perm.ActivityType)),
                PermissionSettingValues.Of(basePermission.IsAllowed, basePermission.IsEnabled),
                group
            );

        public static PermissionManagementModel Of(
            PermissionSettingIdentity settingIdentity,
            PermissionSettingValues permissionSettingValues,
            IntranetMemberGroup group) =>
            new PermissionManagementModel(settingIdentity, permissionSettingValues, group);
    }
}