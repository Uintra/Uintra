using System.Collections.Generic;

namespace uCommunity.Notification.Core.Configuration
{
    public class NotificationTypeConfiguration
    {
        public NotificationTypeEnum NotificationType { get; set; }

        public IEnumerable<NotifierTypeEnum> NotifierTypes { get; set; }
    }
}