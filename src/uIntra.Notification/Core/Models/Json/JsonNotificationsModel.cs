using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Notification.Models.Json
{
    public class JsonNotificationsModel
    {
        public int Count { get; set; }
        public IEnumerable<JsonNotification> Notifications { get; set; }
    }
}
