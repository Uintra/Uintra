using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Reminder
{
    public class ReminderConfiguration
    {
        public IEnumerable<ReminderTypeConfiguration> Configurations { get; set; }

        public ReminderConfiguration()
        {
            Configurations = Enumerable.Empty<ReminderTypeConfiguration>();
        }
    }
}
