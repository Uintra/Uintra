using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Notification.Configuration;

namespace Uintra20.Features.Reminder
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
