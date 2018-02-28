using Uintra.Core.User;
using Uintra.Notification;

namespace Compent.Uintra.Core.Navigation
{
    public class TopMenuViewModel
    {
        public IIntranetUser CurrentUser { get; set; }
        public NotificationListViewModel NotificationList { get; set; }
        public string NotificationsUrl { get; set; }
    }
}