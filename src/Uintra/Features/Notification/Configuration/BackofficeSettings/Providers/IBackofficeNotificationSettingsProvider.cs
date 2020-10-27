using System.Threading.Tasks;
using Uintra.Features.Notification.Models;
using Uintra.Features.Notification.Models.Configuration;
using Uintra.Features.Notification.Models.NotifierTemplates;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface IBackofficeNotificationSettingsProvider
    {
        NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
        Task<NotificationSettingDefaults<T>> GetAsync<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
    }
}
