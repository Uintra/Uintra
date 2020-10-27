using System.Threading.Tasks;
using Uintra.Features.Notification.Models;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Helpers
{
    public interface IBackofficeSettingsReader
    {
        string ReadSettings(ActivityEventNotifierIdentity notificationType);
        Task<string> ReadSettingsAsync(ActivityEventNotifierIdentity notificationType);
    }
}
