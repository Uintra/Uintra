using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Web;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class NotificationController: NotificationControllerBase
    {
        protected override int ItemsPerPage { get; } = 10;

        public NotificationController(IUiNotifierService uiNotifierService, IIntranetUserService<IntranetUser> intranetUserService, INotificationHelper notificationHelper) 
            : base(uiNotifierService, intranetUserService, notificationHelper)
        {
        }
    }
}