namespace Uintra.Notification
{
    public interface INotificationSettingsService
    {
        NotifierSettingsModel GetSettings(ActivityEventIdentity activityEventIdentity);
        NotifierSettingModel<T> Get<T>(ActivityEventNotifierIdentity activityEventNotifierIdentity) where T : INotifierTemplate;
        void Save<T>(NotifierSettingModel<T> settingModel) where T : INotifierTemplate;
    }
}
