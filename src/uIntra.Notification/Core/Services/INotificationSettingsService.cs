using uIntra.Core.Activity;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Models;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationSettingsService
    {
        NotifierSettingsModel Get(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType);
        void Save(NotifierSettingsModel settings);
    }
}
