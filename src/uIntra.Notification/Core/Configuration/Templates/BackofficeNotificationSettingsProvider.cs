namespace uIntra.Notification.Configuration
{
    public interface IBackofficeNotificationSettingsProvider<T>
        where T : INotifierTemplate
    {
        BackofficeNotificationSettingsModel<T> GetBackofficeSettings(ActivityEventIdentity activityEvent);
    }
}