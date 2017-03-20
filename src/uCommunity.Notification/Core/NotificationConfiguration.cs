using System.Collections.Generic;
using System.Linq;
using uCommunity.Notification.Notifier;

namespace uCommunity.Notification
{
    public class NotificationConfiguration
    {        
        public NotifierTypeEnum DefaultNotifier { get; set; }

        public IEnumerable<NotificationTypeConfiguration> NotificationTypeConfigurations { get; set; }

        public IEnumerable<NotifierConfiguration> NotifierConfigurations { get; set; }

        public NotificationConfiguration()
        {
            NotificationTypeConfigurations = Enumerable.Empty<NotificationTypeConfiguration>();
            NotifierConfigurations = Enumerable.Empty<NotifierConfiguration>();
        }

    }
}
