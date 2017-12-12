using System.Linq;
using System.Net.Http.Formatting;
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
            var node = _notificationSettingsTreeProvider.GetSettingsTree().Where(t => t.Id == id).First();
            var mappedNode = MapNode(node, queryStrings);
            return mappedNode;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }

        protected TreeNodeCollection MapNode(Tree<TreeNodeModel> tree, FormDataCollection queryStrings)
        {
            var mapped = tree.Children.Select(ch =>
                CreateTreeNode(ch.Value.Id, tree.Value.Id, queryStrings, ch.Value.Name, ch.Value.Icon, ch.Children.Any(), ch.Value.ViewPath));
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