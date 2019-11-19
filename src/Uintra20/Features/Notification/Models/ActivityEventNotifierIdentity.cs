using System;

namespace Uintra20.Features.Notification.Models
{
    public class ActivityEventNotifierIdentity
    {
        public ActivityEventIdentity Event { get; }
        public Enum NotifierType { get; }

        public ActivityEventNotifierIdentity(Enum activityType, Enum notificationType, Enum notifierType)
            : this(new ActivityEventIdentity(activityType, notificationType), notifierType)
        { }

        public ActivityEventNotifierIdentity(ActivityEventIdentity @event, Enum notifierType)
        {
            Event = @event;
            NotifierType = notifierType;
        }
    }
}