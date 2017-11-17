using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationSettingsService
    {
        NotifierSettingsModel Get(ActivityEventIdentity activityEventIdentity);
        void Save<T>(NotifierSettingModel<T> setting);
    }
}
