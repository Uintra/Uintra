using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class ActivityEventIdentity
    {
        public IIntranetType ActivityType { get; }
        public IIntranetType NotificationType { get; }

        public ActivityEventIdentity(IIntranetType activityType, IIntranetType notificationType)
        {
            ActivityType = activityType;
            NotificationType = notificationType;
        }
    }
}
