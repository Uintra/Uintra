using System;

namespace Uintra20.Features.Notification.Models
{
    public class ActivityEventIdentity
    {
        public Enum ActivityType { get; }
        public Enum NotificationType { get; }

        public ActivityEventIdentity(Enum activityType, Enum notificationType)
        {
            ActivityType = activityType;
            NotificationType = notificationType;
        }
    }
}