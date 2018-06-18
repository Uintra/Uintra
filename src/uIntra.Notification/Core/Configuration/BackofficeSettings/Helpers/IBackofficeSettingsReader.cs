namespace Uintra.Notification.Configuration
{
    public interface IBackofficeSettingsReader
    {
        string ReadSettings(ActivityEventNotifierIdentity notificationType);
    }
}