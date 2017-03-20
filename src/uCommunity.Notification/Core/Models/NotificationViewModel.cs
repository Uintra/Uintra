using System;

namespace uCommunity.Notification.Models
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public dynamic Value { get; set; }
    }
}