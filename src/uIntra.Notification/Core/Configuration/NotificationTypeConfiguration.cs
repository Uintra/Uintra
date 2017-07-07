using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    public class NotificationTypeConfiguration
    {
        public string NotificationType { get; set; }

        public IEnumerable<NotifierTypeEnum> NotifierTypes { get; set; }
    }
}