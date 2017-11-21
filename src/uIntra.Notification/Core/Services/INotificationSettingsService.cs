using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationSettingsService   
    {
        NotifierSettingsModel GetAll(ActivityEventIdentity activityEventIdentity);
        NotifierSettingModel<EmailNotifierTemplate> GetEmailNotifierSettings(ActivityEventNotifierIdentity activityEventNotifierIdentity);
        NotifierSettingModel<UiNotifierTemplate> GetUiNotifierSettings(ActivityEventNotifierIdentity activityEventNotifierIdentity);
        void SaveUiNotifierSettings(NotifierSettingModel<UiNotifierTemplate> settingModel);
        void SaveEmailNotifierSettings(NotifierSettingModel<EmailNotifierTemplate> settingModel);
    }
}
