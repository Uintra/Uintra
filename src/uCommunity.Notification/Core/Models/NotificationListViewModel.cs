using System.Collections.Generic;

namespace uCommunity.Notification.Models
{
    public class NotificationListViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; }
    }
}