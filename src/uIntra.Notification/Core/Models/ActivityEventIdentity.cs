using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class ActivityEventIdentity
    {
        public IIntranetType ActivityType { get; }
        public Enum NotificationType { get; }

        public ActivityEventIdentity(IIntranetType activityType, Enum notificationType)
        {
            ActivityType = activityType;
            NotificationType = notificationType;
        }
    }
}
