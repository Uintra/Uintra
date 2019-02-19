using LanguageExt;
using System;

namespace Uintra.Core.Permissions.Models
{
    public class BasePermissionUpdateModel
    {
        public IntranetMemberGroup Group { get; }
        public Enum Action { get; }
        public Option<Enum> ActivityType { get; }
        public PermissionSettingValues SettingValues { get; }

        private BasePermissionUpdateModel(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            Enum action,
            Option<Enum> activityType)
        {
            Group = group;
            Action = action;
            ActivityType = activityType;
            SettingValues = settingValues;
        }

        public static BasePermissionUpdateModel Of(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            PermissionSettingIdentity identity) =>
            new BasePermissionUpdateModel(group, settingValues, identity.ActionType, identity.ActivityType);
    }
}
