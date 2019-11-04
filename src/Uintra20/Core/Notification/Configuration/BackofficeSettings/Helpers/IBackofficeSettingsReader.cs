using System.Threading.Tasks;

namespace Uintra20.Core.Notification.Configuration
{
    public interface IBackofficeSettingsReader
    {
        string ReadSettings(ActivityEventNotifierIdentity notificationType);
        Task<string> ReadSettingsAsync(ActivityEventNotifierIdentity notificationType);
    }
}
