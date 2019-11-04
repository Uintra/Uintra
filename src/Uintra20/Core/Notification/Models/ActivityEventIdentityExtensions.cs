using System;

namespace Uintra20.Core.Notification
{
    public static class ActivityEventIdentityExtensions
    {
        public static ActivityEventNotifierIdentity AddNotifierIdentity(this ActivityEventIdentity activityEventIdentity, Enum notifierType)
        {
            return new ActivityEventNotifierIdentity(activityEventIdentity, notifierType);
        }
    }
}