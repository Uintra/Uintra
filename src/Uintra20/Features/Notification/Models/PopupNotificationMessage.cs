using System;

namespace Uintra20.Features.Notification.Models
{
    public class PopupNotificationMessage : INotificationMessage
    {
        public Enum NotificationType { get; set; }
        public Guid ReceiverId { get; set; }
        public string Message { get; set; }
    }
}