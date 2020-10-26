using System.Collections.Generic;

namespace Uintra20.Features.Notification.Json
{
    public class JsonNotificationsModel
    {
        public int Count { get; set; }
        public IEnumerable<JsonNotification> Notifications { get; set; }
    }
}
