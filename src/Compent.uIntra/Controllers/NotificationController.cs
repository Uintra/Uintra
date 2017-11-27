using uIntra.Core.Links;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Web;

namespace Compent.uIntra.Controllers
{
    public class NotificationController : NotificationControllerBase
    {
        protected override int ItemsPerPage { get; } = 10;

        public NotificationController(
            IUiNotificationService uiNotificationService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationContentProvider notificationContentProvider,
            IIntranetUserContentProvider intranetUserContentProvider,
            IProfileLinkProvider profileLinkProvider)
            : base(uiNotificationService, intranetUserService, notificationContentProvider, profileLinkProvider)
        {
        }
    }
}