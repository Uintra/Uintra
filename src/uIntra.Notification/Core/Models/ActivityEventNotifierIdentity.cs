using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class ActivityEventNotifierIdentity
    {
        public ActivityEventIdentity Event { get; }
        public IIntranetType NotifierType { get; }

        public ActivityEventNotifierIdentity(IIntranetType activityType, IIntranetType notificationType, IIntranetType notifierType)
            : this(new ActivityEventIdentity(activityType, notificationType), notifierType)
        { }

        public ActivityEventNotifierIdentity(ActivityEventIdentity @event, IIntranetType notifierType)
        {
            Event = @event;
            NotifierType = notifierType;
        }
    }
}
