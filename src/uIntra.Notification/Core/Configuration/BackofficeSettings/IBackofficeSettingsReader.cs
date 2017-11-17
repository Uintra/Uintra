namespace uIntra.Notification.Configuration
{
    public interface IBackofficeSettingsReader
    {
        string ReadTemplate(ActivityEventNotifierIdentity notificationType);
    }
}