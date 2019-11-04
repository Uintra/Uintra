﻿using System;

namespace Uintra20.Core.Notification
{
    public class PopupNotificationMessage : INotificationMessage
    {
        public Enum NotificationType { get; set; }
        public Guid ReceiverId { get; set; }
        public string Message { get; set; }
    }
}