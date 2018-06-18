using BCLExtensions.Trees;

namespace uIntra.Notification.Configuration
{
    public interface INotificationSettingsTreeProvider
    {
        ITree<TreeNodeModel> GetSettingsTree();
    }
}
