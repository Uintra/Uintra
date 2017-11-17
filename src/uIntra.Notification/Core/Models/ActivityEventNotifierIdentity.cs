using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class ActivityEventNotifierIdentity
    {
        public ActivityEventIdentity Event { get; }
        public NotifierTypeEnum NotifierType { get; }

        public ActivityEventNotifierIdentity(ActivityEventIdentity @event, NotifierTypeEnum notifierTypeEnum)
        {
            Event = @event;
            NotifierType = notifierTypeEnum;
        }
    }
}
