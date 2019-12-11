using Compent.Extensions.Trees;
using System.Linq;
using System.Net.Http.Formatting;
using Uintra.Notification.Configuration;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Models;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Uintra20.Features.Notification.Controllers
{
    [Tree("notificationSettings", "notificationSettingsTree", TreeTitle = "Notification Settings", TreeGroup = "notificationSettingsTreeGroup")]
    [PluginController("NotificationSettings")]
    public class NotificationSettingsTreeController : TreeController
    {
        private readonly INotificationSettingsTreeProvider _notificationSettingsTreeProvider;

        public NotificationSettingsTreeController(INotificationSettingsTreeProvider notificationSettingsTreeProvider)
        {
            _notificationSettingsTreeProvider = notificationSettingsTreeProvider;
        }
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var node = _notificationSettingsTreeProvider
                .GetSettingsTree()
                .FirstOrDefault(t => t.Id == id);

            var mappedNode = MapNode(node, queryStrings);
            return mappedNode;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }

        protected TreeNodeCollection MapNode(ITree<TreeNodeModel> tree, FormDataCollection queryStrings)
        {
            var mapped = tree
                .GetChildren()
                .Select(ch =>
                    CreateTreeNode(
                        ch.Value.Id,
                        tree.Value.Id,
                        queryStrings,
                        ch.Value.Name,
                        ch.Value.Icon,
                        ch.IsNode(),
                        ch.Value.ViewPath));

            var col = new TreeNodeCollection();
            col.AddRange(mapped);
            return col;
        }
    }

}