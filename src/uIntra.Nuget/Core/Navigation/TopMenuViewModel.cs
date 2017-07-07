using uIntra.Core.User;
using uIntra.Notification;

namespace Compent.uIntra.Core.Navigation
{
    public class TopMenuViewModel
    {
        public IIntranetUser CurrentUser { get; set; }
        public NotificationListViewModel NotificationList { get; set; }
        public string NotificationsUrl { get; set; }
    }
}