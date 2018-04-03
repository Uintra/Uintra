using Localization.Umbraco.Attributes;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Web;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    public class NotificationController : NotificationControllerBase
    {
        protected override int ItemsPerPage { get; } = 10;

        public NotificationController(
            IUiNotificationService uiNotificationService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationContentProvider notificationContentProvider,
            IIntranetUserContentProvider intranetUserContentProvider,
            IProfileLinkProvider profileLinkProvider,
            IPopupNotificationService popupNotificationService)
            : base(uiNotificationService, intranetUserService, notificationContentProvider, profileLinkProvider, popupNotificationService)
        {
        }
    }
}