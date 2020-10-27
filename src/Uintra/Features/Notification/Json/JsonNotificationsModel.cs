using System.Collections.Generic;

namespace Uintra.Features.Notification.Json
{
    public class JsonNotificationsModel
    {
        public int Count { get; set; }
        public IEnumerable<JsonNotification> Notifications { get; set; }
    }
}
