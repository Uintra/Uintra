namespace Uintra.Notification.Configuration
{
    public interface IBackofficeNotificationSettingsProvider 
    {
        NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
    }
}