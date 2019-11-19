using System.Threading.Tasks;
using Uintra20.Features.Notification.Models;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Helpers
{
    public interface IBackofficeSettingsReader
    {
        string ReadSettings(ActivityEventNotifierIdentity notificationType);
        Task<string> ReadSettingsAsync(ActivityEventNotifierIdentity notificationType);
    }
}
