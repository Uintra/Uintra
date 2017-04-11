using System.Collections.Generic;

namespace uCommunity.Notification.Core.Models
{
    public class NotificationListViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; }

        public bool BlockScrolling { get; set; }
    }
}