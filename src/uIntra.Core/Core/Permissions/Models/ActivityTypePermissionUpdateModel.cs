using System;

namespace Uintra.Core.Permissions.Models
{
    public class ActivityTypePermissionCreateModel
    {
        public Enum ActivityType { get; }
        public Guid BasePermissionId { get; }

        private ActivityTypePermissionCreateModel(
            Guid basePermissionId,
            Enum activityType)
        {
            BasePermissionId = basePermissionId;
            ActivityType = activityType;
        }

        public static ActivityTypePermissionCreateModel Of(
            Guid basePermissionId,
            Enum activityType) =>
            new ActivityTypePermissionCreateModel(basePermissionId, activityType);
    }
}
