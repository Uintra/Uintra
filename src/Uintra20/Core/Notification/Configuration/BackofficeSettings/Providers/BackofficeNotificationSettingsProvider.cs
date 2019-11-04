using System.Threading.Tasks;
using Uintra20.Core.Extensions;

namespace Uintra20.Core.Notification.Configuration
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

        public async Task<NotificationSettingDefaults<T>> GetAsync<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate
        {
            var result = (await _backofficeSettingsReader.ReadSettingsAsync(identity)).Deserialize<NotificationSettingDefaults<T>>();
            return result;
        }
    }
}