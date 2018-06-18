using Uintra.Core.TypeProviders;
using Uintra.Notification;
using Uintra.Notification.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class NotificationSettingsApiController : NotificationSettingsApiControllerBase
    {
        public NotificationSettingsApiController(
            INotificationSettingsService notificationSettingsService,
            IActivityTypeProvider activityTypeProvider,
            INotificationTypeProvider notificationTypeProvider,
            INotifierTypeProvider notifierTypeProvider) 
            : base(notificationSettingsService, activityTypeProvider, notificationTypeProvider, notifierTypeProvider)
        {
        }
    }
}