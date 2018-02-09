using System.Collections.Generic;

namespace Uintra.Notification
{
    public class NotificationListViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; }
        public bool BlockScrolling { get; set; }
    }
}