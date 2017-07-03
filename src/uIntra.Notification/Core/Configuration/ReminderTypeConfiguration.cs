using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    public class ReminderTypeConfiguration
    {
        public ReminderTypeEnum Type { get; set; }

        public IEnumerable<string> NotificationTypes { get; set; }

        public int Time { get; set; }

        public ReminderTypeConfiguration()
        {
            NotificationTypes = Enumerable.Empty<string>();
        }
    }
}
