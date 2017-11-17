using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Core.Models
{
    public class ActivityEventNotifierIdentity
    {
        public IntranetActivityTypeEnum ActivityType { get; }
        public NotificationTypeEnum NotificationType { get;}
        public NotifierTypeEnum NotifierType { get;}

        public ActivityEventNotifierIdentity(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType, NotifierTypeEnum notifierTypeEnum)
        {
            ActivityType = activityType;
            NotificationType = notificationType;
            NotifierType = notifierTypeEnum;
        }
    }
}
