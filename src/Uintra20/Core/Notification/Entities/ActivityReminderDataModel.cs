﻿using System;
using Uintra20.Core.Notification.Base;

namespace Uintra20.Core.Notification
{
    public class ActivityReminderDataModel : INotifierDataValue
    {
        public Enum NotificationType { get; set; }
        public Enum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public DateTime StartDate { get; set; }
    }
}