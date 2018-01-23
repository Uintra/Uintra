using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class ActivityEventNotifierIdentity
    {
        public ActivityEventIdentity Event { get; }
        public Enum NotifierType { get; }

        public ActivityEventNotifierIdentity(IIntranetType activityType, IIntranetType notificationType, Enum notifierType)
            : this(new ActivityEventIdentity(activityType, notificationType), notifierType)
        { }

        public ActivityEventNotifierIdentity(ActivityEventIdentity @event, Enum notifierType)
        {
            Event = @event;
            NotifierType = notifierType;
        }
    }
}
