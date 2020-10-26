using System.Threading.Tasks;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.Configuration;
using Uintra20.Features.Notification.Models.NotifierTemplates;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface IBackofficeNotificationSettingsProvider
    {
        NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
        Task<NotificationSettingDefaults<T>> GetAsync<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
    }
}
