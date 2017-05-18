using uCommunity.Core.User;
using uCommunity.Notification.Core.Models;

namespace Compent.uCommunity.Core.Navigation
{
    public class TopMenuViewModel
    {
        public IIntranetUser CurrentUser { get; set; }
        public NotificationListViewModel NotificationList { get; set; }
        public string NotificationsUrl { get; set; }
    }
}