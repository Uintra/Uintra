using Compent.Extensions.Trees;
using Uintra.Features.Notification.Configuration.BackofficeSettings.Models;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationSettingsTreeProvider
    {
        ITree<TreeNodeModel> GetSettingsTree();
    }
}
