using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class ActivityEventIdentity
    {
        public IntranetActivityTypeEnum ActivityType { get; }
        public NotificationTypeEnum NotificationType { get; }
        public ActivityEventIdentity(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            ActivityType = activityType;
            NotificationType = notificationType;
        }

        public ActivityEventNotifierIdentity AddNotifierIdentity(NotifierTypeEnum notifierType)
        {
           return new ActivityEventNotifierIdentity(this, notifierType);
        }
    }
}
