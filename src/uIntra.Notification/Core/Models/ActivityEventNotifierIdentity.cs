using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class ActivityEventNotifierIdentity
    {
        public ActivityEventIdentity Event { get; }
        public IIntranetType NotifierType { get; }

        public ActivityEventNotifierIdentity(ActivityEventIdentity @event, IIntranetType notifierType)
        {
            Event = @event;
            NotifierType = notifierType;
        }
    }
}
