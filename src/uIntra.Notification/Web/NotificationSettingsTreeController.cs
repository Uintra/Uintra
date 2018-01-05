using System.Linq;
using System.Net.Http.Formatting;
using BCLExtensions.Trees;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;
using umbraco.businesslogic;
using umbraco.interfaces;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace uIntra.Notification.Web
{
    [Umbraco.Web.Trees.Tree("NotificationSettings", "NotificationSettingsTree", "Notification Settings")]
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
            var node = _notificationSettingsTreeProvider.GetSettingsTree().FirstOrDefault(t => t.Id == id);
            var mappedNode = MapNode(node, queryStrings);
            return mappedNode;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }

        protected TreeNodeCollection MapNode(ITree<TreeNodeModel> tree, FormDataCollection queryStrings)
        {
            var mapped = tree.GetChildren().Select(ch =>
                CreateTreeNode(ch.Value.Id, tree.Value.Id, queryStrings, ch.Value.Name, ch.Value.Icon, ch.IsNode(), ch.Value.ViewPath));
            var col = new TreeNodeCollection();
            col.AddRange(mapped);
            return col;
        }
    }

    [Application("NotificationSettings", "NotificationSettings", "icon-file-cabinet")]
    public class NotificationSettingsApplication : IApplication
    {

    }
}