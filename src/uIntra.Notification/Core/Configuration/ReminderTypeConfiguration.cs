using System.Collections.Generic;
using System.Linq;

namespace uIntra.Notification.Core.Configuration
{
    public class ReminderTypeConfiguration
    {
        public ReminderTypeEnum Type { get; set; }

        public IEnumerable<NotificationTypeEnum> NotificationTypes { get; set; }

        public int Time { get; set; }

        public ReminderTypeConfiguration()
        {
            NotificationTypes = Enumerable.Empty<NotificationTypeEnum>();
        }
    }
}
