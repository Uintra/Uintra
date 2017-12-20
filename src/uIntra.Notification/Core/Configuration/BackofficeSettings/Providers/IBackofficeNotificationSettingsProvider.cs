namespace uIntra.Notification.Configuration
{
    public interface IBackofficeNotificationSettingsProvider 
    {
        NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
    }
}