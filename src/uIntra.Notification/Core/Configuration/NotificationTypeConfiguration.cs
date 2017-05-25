using System.Collections.Generic;

namespace uIntra.Notification.Core.Configuration
{
    public class NotificationTypeConfiguration
    {
        public NotificationTypeEnum NotificationType { get; set; }

        public IEnumerable<NotifierTypeEnum> NotifierTypes { get; set; }
    }
}