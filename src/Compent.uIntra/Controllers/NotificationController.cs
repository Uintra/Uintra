using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Web;

namespace Compent.uIntra.Controllers
{
    public class NotificationController : NotificationControllerBase
    {
        protected override int ItemsPerPage { get; } = 10;

        public NotificationController(
            IUiNotifierService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationHelper notificationHelper,
            IIntranetUserContentHelper intranetUserContentHelper)
            : base(uiNotifierService, intranetUserService, notificationHelper, intranetUserContentHelper)
        {
        }
    }
}