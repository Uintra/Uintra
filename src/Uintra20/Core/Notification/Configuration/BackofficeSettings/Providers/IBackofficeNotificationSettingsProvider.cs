using System.Threading.Tasks;

namespace Uintra20.Core.Notification.Configuration
{
    public interface IBackofficeNotificationSettingsProvider
    {
        NotificationSettingDefaults<T> Get<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
        Task<NotificationSettingDefaults<T>> GetAsync<T>(ActivityEventNotifierIdentity identity) where T : INotifierTemplate;
    }
}
