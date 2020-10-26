using Compent.Extensions.Trees;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Models;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationSettingsTreeProvider
    {
        ITree<TreeNodeModel> GetSettingsTree();
    }
}
