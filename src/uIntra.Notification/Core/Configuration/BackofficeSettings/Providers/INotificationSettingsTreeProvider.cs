using Compent.Extensions.Trees;

namespace Uintra.Notification.Configuration
{
    public interface INotificationSettingsTreeProvider
    {
        ITree<TreeNodeModel> GetSettingsTree();
    }
}
