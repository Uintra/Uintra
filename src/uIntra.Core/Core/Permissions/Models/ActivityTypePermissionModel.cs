using System;
using System.Collections.Generic;
using Uintra.Core.Permissions.Sql;

namespace Uintra.Core.Permissions.Models
{
    public class ActivityTypePermissionModel
    {
        public Guid Id { get; }
        public Enum ActivityType { get; }
        public Guid BasePermissionId { get; }

        private ActivityTypePermissionModel(Guid id, Enum activityType, Guid basePermissionId)
        {
            Id = id;
            ActivityType = activityType;
            BasePermissionId = basePermissionId;
        }

        public static ActivityTypePermissionModel Of(IDictionary<int, Enum> activityTypeDictionary, PermissionActivityTypeEntity entity) =>
            new ActivityTypePermissionModel(
                entity.Id,
                activityTypeDictionary[entity.ActivityTypeId],
                entity.PermissionEntityId);
    }
}
