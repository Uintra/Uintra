using Uintra.Core.Extensions;

namespace Uintra.Notification.Configuration
{
    public class BackofficeNotificationSettingsProvider : IBackofficeNotificationSettingsProvider
    {
        private readonly IBackofficeSettingsReader _backofficeSettingsReader;

        public BackofficeNotificationSettingsProvider(IBackofficeSettingsReader backofficeSettingsReader)
        {
            _backofficeSettingsReader = backofficeSettingsReader;
        }

        public NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate
        {
            var result = _backofficeSettingsReader.ReadSettings(identity).Deserialize<NotificationSettingDefaults<T>>();
            return result;
        }
    }
}