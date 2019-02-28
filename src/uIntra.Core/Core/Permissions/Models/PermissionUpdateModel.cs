using System;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionUpdateModel
    {
        public IntranetMemberGroup Group { get; }
        public Enum Action { get; }
        public Enum ResourceType { get; }
        public PermissionSettingValues SettingValues { get; }

        private PermissionUpdateModel(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            Enum actionType,
            Enum resourceType)
        {
            Group = group;
            Action = actionType;
            ResourceType = resourceType;
            SettingValues = settingValues;
        }

        public static PermissionUpdateModel Of(
            IntranetMemberGroup group,
            PermissionSettingValues settingValues,
            PermissionSettingIdentity identity) =>
            new PermissionUpdateModel(group, settingValues, identity.ActionType, identity.ResourceType);
    }
}
