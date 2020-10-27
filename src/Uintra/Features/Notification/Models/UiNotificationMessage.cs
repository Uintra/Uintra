﻿using System;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Notification.Models
{
    public class UiNotificationMessage : INotificationMessage
    {
        public Enum NotificationType { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid? NotifierId { get; set; }
        public UintraLinkModel Url { get; set; }
        public string Message { get; set; }
        public string NotifierPhotoUrl { get; set; }
        public string DesktopMessage { get; set; }
        public string DesktopTitle { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public bool IsDesktopNotificationEnabled { get; set; }
    }
}