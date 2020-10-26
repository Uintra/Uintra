using System.Threading.Tasks;
using UBaseline.Core.Extensions;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Helpers;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.Configuration;
using Uintra20.Features.Notification.Models.NotifierTemplates;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
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