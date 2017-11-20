namespace uIntra.Notification.Configuration
{
    public interface IBackofficeNotificationSettingsProvider<T>
        where T : INotifierTemplate
    {
        NotificationSettingDefaults<T> GetSettings(ActivityEventIdentity activityEvent);
    }
}