namespace uIntra.Notification.Configuration
{
    public interface IBackofficeSettingsReader
    {
        string ReadSettings(ActivityEventNotifierIdentity notificationType);
    }
}