using LanguageExt;
using System;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionUpdateModel
    {
        public IntranetMemberGroup Group { get; }
        public Enum Action { get; }
        public Option<Enum> ActivityType { get; }
        public PermissionSettingValues SettingValues { get; }

        private PermissionUpdateModel(
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

        public static PermissionUpdateModel Of(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            PermissionSettingIdentity identity) =>
            new PermissionUpdateModel(group, settingValues, identity.ActionType, identity.ActivityType);
    }
}
