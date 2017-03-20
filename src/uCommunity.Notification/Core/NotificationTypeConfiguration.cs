using System.Collections.Generic;
using uCommunity.Notification.Notifier;

namespace uCommunity.Notification
{
    public class NotificationTypeConfiguration
    {
        public NotificationTypeEnum NotificationType { get; set; }

        public IEnumerable<NotifierTypeEnum> NotifierTypes { get; set; }
    }
}