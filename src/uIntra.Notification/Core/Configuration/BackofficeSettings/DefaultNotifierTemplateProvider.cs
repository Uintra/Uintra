using uIntra.Core.Extensions;
using uIntra.Notification.Core;

namespace uIntra.Notification.Configuration
{
    public class
        BackofficeNotificationSettingsProvider : IBackofficeNotificationSettingsProvider
    {
        private readonly IBackofficeSettingsReader _backofficeSettingsReader;
        private readonly INotifierTypeProvider _notifierTypeProvider;

        public BackofficeNotificationSettingsProvider(IBackofficeSettingsReader backofficeSettingsReader, INotifierTypeProvider notifierTypeProvider)
        {
            _backofficeSettingsReader = backofficeSettingsReader;
            _notifierTypeProvider = notifierTypeProvider;
        }

        public NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate
        {
            var result = _backofficeSettingsReader.ReadSettings(identity).Deserialize<NotificationSettingDefaults<T>>();
            return result;
        }
    }
}