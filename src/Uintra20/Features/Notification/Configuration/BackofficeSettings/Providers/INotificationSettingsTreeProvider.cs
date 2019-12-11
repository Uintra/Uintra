using Compent.Extensions.Trees;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Models;

namespace Uintra.Notification.Configuration
{
    public interface INotificationSettingsTreeProvider
    {
        ITree<TreeNodeModel> GetSettingsTree();
    }
}
