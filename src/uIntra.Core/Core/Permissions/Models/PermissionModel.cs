using LanguageExt;
using System;
using System.Collections.Generic;
using Uintra.Core.Permissions.Sql;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionModel
    {
        public Guid Id { get; }
        public IntranetMemberGroup Group { get; }
        public Enum ActionType { get; }
        public Option<Enum> ActivityType { get; }
        public bool IsAllowed { get; }
        public bool IsEnabled { get; }

        private PermissionModel(Guid id, IntranetMemberGroup group, Enum permissionType, Option<Enum> activityType, bool isAllowed, bool isEnabled)
        {
            Id = id;
            Group = group;
            ActionType = permissionType;
            ActivityType = activityType;
            IsAllowed = isAllowed;
            IsEnabled = isEnabled;
        }

        public static PermissionModel Of(IDictionary<int,IntranetMemberGroup> groupDictionary,
            IDictionary<int, Enum> actionDictionary,
            IDictionary<int, Enum> activityDictionary,
            PermissionEntity entity) =>
            new PermissionModel(
                entity.Id,
                groupDictionary[entity.IntranetMemberGroupId],
                actionDictionary[entity.IntranetActionId],
                //entity.ActivityTypeId.ToOption().Map(i => activityDictionary[i]),
                Some(entity.ActivityTypeId).Some(i => activityDictionary[i]).None(() => null),
                entity.IsAllowed,
                entity.IsEnabled);

        public static PermissionModel Of(PermissionSettingIdentity identity, PermissionSettingValues values,
            IntranetMemberGroup group) =>
            new PermissionModel(
                Guid.NewGuid(),
                group,
                identity.ActionType,
                identity.ActivityType,
                values.IsAllowed,
                values.IsEnabled);
    }
}