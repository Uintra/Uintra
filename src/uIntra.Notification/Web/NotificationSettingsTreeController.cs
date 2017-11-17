using System;
using System.Linq;
using System.Net.Http.Formatting;
using uIntra.Core.Activity;
using uIntra.Core.Core.Extensions;
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
        private const string CategoryView = "NotificationSettings/NotificationSettingsTree/Category/edit";
        private const string SettingView = "NotificationSettings/NotificationSettingsTree/Settings/edit";

        private readonly Tree<TreeNodeModel> _tree;

        public NotificationSettingsTreeController()
        {
            var icon = "icon-circle-dotted";

            _tree = Node("-1", "root", icon, CategoryView,
                WithUrlIdentity(Node(IntranetActivityTypeEnum.Bulletins, IntranetActivityTypeEnum.Bulletins, icon, CategoryView,
                    Node(NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentAdded, icon, SettingView),
                    Node(NotificationTypeEnum.ActivityLikeAdded, NotificationTypeEnum.ActivityLikeAdded, icon, SettingView))),
                WithUrlIdentity(Node(IntranetActivityTypeEnum.News, IntranetActivityTypeEnum.News, icon, CategoryView,
                    Node(NotificationTypeEnum.News, NotificationTypeEnum.News, icon, SettingView),
                    Node(NotificationTypeEnum.CommentEdited, NotificationTypeEnum.CommentEdited, icon, SettingView))),
                WithUrlIdentity(Node(IntranetActivityTypeEnum.Events, IntranetActivityTypeEnum.Events, icon, CategoryView,
                    Node(NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentAdded, icon, SettingView),
                    Node(NotificationTypeEnum.ActivityLikeAdded, NotificationTypeEnum.CommentAdded, icon, SettingView))));
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var node = _tree.Where(t => t.Id == id).First();
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

        protected Tree<TreeNodeModel> WithUrlIdentity(Tree<TreeNodeModel> tree)
        {
            TreeNodeModel AddNotificationTypeParameter(TreeNodeModel model) =>
                model.WithViewPath(model.ViewPath + "?notificationType=" + model.Id);

            TreeNodeModel AddActivityTypeParameter(TreeNodeModel model, string type) =>
                model.WithViewPath(model.ViewPath + "&activityType=" + type);

            return tree.TreeCatamorphism(
                leaf => Node(AddNotificationTypeParameter(leaf)),
                (node, children) => Node(node.Id, node.Name, node.Icon, node.ViewPath, children
                    .Select(c => Node(AddActivityTypeParameter(c.Value, node.Id), c.Children.ToArray())).ToArray()));
        }

        protected Tree<TreeNodeModel> Node(object id, object name, string icon, string viewPath, params Tree<TreeNodeModel>[] children)
        {
            return new Tree<TreeNodeModel>(new TreeNodeModel(id.ToString(), name.ToString(), icon, viewPath), children);
        }

        protected Tree<TreeNodeModel> Node(TreeNodeModel treeNodeModel, params Tree<TreeNodeModel>[] children)
        {
            return new Tree<TreeNodeModel>(treeNodeModel, children);
        }

        protected class TreeNodeModel
        {
            public string Id { get; }
            public string Name { get; }
            public string Icon { get; }
            public string ViewPath { get; }

            public TreeNodeModel(string id, string name, string icon, string viewPath)
            {
                Id = id;
                Name = name;
                Icon = icon;
                ViewPath = viewPath;
            }

            public TreeNodeModel WithViewPath(string viewPath) => new TreeNodeModel(Id, Name, Icon, viewPath);
        }

    }

    [Application("NotificationSettings", "NotificationSettings", "icon-file-cabinet")]
    public class NotificationSettingsApplication : IApplication
    {

    }
}