using uIntra.Core.TypeProviders;
using uIntra.Notification;
using uIntra.Notification.Core.Services;
using uIntra.Notification.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class NotificationSettingsApiController : NotificationSettingsApiControllerBase
    {
        public NotificationSettingsApiController(
            INotificationSettingsService notificationSettingsService,
            IActivityTypeProvider activityTypeProvider,
            INotificationTypeProvider notificationTypeProvider) : base(notificationSettingsService, activityTypeProvider, notificationTypeProvider)
        {
        }
    }
}