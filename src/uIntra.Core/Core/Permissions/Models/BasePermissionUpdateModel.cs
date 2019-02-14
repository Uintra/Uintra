using System;

namespace Uintra.Core.Permissions.Models
{
    public class BasePermissionUpdateModel
    {
        public IntranetMemberGroup Group { get; }
        public Enum Action { get; }
        public PermissionSettingValues SettingValues { get; }

        private BasePermissionUpdateModel(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            Enum action)
        {
            Group = group;
            Action = action;
            SettingValues = settingValues;
        }

        public static BasePermissionUpdateModel Of(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            Enum action) =>
            new BasePermissionUpdateModel(group, settingValues, action);
    }
}
