﻿using System;

namespace Uintra20.Core.Notification
{
    public class ActivityEventIdentity
    {
        public Enum ActivityType { get; }
        public Enum NotificationType { get; }

        public ActivityEventIdentity(Enum activityType, Enum notificationType)
        {
            ActivityType = activityType;
            NotificationType = notificationType;
        }
    }
}