﻿using System.Collections.Generic;

namespace Uintra.Features.Notification.Configuration
{
    public class NotificationTypeConfiguration
    {
        public string NotificationType { get; set; }

        public IEnumerable<NotifierTypeEnum> NotifierTypes { get; set; }
    }
}