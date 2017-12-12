using uIntra.Core.Extensions;

namespace uIntra.Notification.Configuration
{
    public interface INotificationSettingsTreeProvider
    {
        Tree<TreeNodeModel> GetSettingsTree();
    }
}
