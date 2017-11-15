using uIntra.Notification.Core.Services;
using uIntra.Notification.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class NotificationSettingsApiController: NotificationSettingsApiControllerBase
    {
        public NotificationSettingsApiController(INotificationSettingsService notificationSettingsService) : base(notificationSettingsService)
        {
        }
    }
}