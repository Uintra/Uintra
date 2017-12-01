using System;
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
        private const string CategoryView = "NotificationSettings/NotificationSettingsTree/Category/edit";
        private const string SettingView = "NotificationSettings/NotificationSettingsTree/Settings/edit";
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        private readonly Tree<TreeNodeModel> _tree;

        public NotificationSettingsTreeController(IActivityTypeProvider activityTypeProvider,
            INotificationTypeProvider notificationTypeProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;

            var bulletinSettings = ActivityCategory(IntranetActivityTypeEnum.Bulletins, IntranetActivityTypeEnum.Bulletins,
                NotificationSetting(NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentAdded),
                NotificationSetting(NotificationTypeEnum.CommentEdited, NotificationTypeEnum.CommentEdited),
                NotificationSetting(NotificationTypeEnum.CommentReplied, NotificationTypeEnum.CommentReplied),
                NotificationSetting(NotificationTypeEnum.ActivityLikeAdded, NotificationTypeEnum.ActivityLikeAdded)
            );

            var newsSettings = ActivityCategory(IntranetActivityTypeEnum.News, IntranetActivityTypeEnum.News,
                NotificationSetting(NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentAdded),
                NotificationSetting(NotificationTypeEnum.CommentReplied, NotificationTypeEnum.CommentReplied),
                NotificationSetting(NotificationTypeEnum.ActivityLikeAdded, NotificationTypeEnum.ActivityLikeAdded),
                NotificationSetting(NotificationTypeEnum.ActivityLikeAdded, NotificationTypeEnum.ActivityLikeAdded)
            );

            var eventSettings = ActivityCategory(IntranetActivityTypeEnum.Events, IntranetActivityTypeEnum.Events,
                NotificationSetting(NotificationTypeEnum.EventUpdated, NotificationTypeEnum.EventUpdated),
                NotificationSetting(NotificationTypeEnum.EventHided, NotificationTypeEnum.EventHided),
                NotificationSetting(NotificationTypeEnum.BeforeStart, NotificationTypeEnum.BeforeStart),
                NotificationSetting(NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentAdded),
                NotificationSetting(NotificationTypeEnum.CommentEdited, NotificationTypeEnum.CommentEdited),
                NotificationSetting(NotificationTypeEnum.CommentReplied, NotificationTypeEnum.CommentReplied),
                NotificationSetting(NotificationTypeEnum.ActivityLikeAdded, NotificationTypeEnum.ActivityLikeAdded)
            );

            _tree = RootNode.WithChildren(
                    bulletinSettings,
                    newsSettings,
                    eventSettings)
                .Select(n => n.WithViewPath(n.ViewPath + "&id=" + n.Id));
        }

        protected Tree<TreeNodeModel> ActivityCategory(IntranetActivityTypeEnum activityType, object name, params Tree<TreeNodeModel>[] children) =>
            Node(GetIntranetType(activityType).Id, name, "icon-navigation-right", CategoryView, children);

        protected Tree<TreeNodeModel> NotificationSetting(NotificationTypeEnum notificationType, object name) =>
            Node(GetIntranetType(notificationType).Id, name, "icon-folder-outline", SettingView);

        protected Tree<TreeNodeModel> RootNode =>
            Node("-1", "root", "", CategoryView);

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
                model.WithViewPath(model.ViewPath + "&activityType=" + type).WithId($"{model.Id}{type}");

            return tree.TreeCatamorphism(
                leaf => Node(AddNotificationTypeParameter(leaf).WithIcon("icon-navigation-right")),
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
            public TreeNodeModel WithId(string id) => new TreeNodeModel(id, Name, Icon, ViewPath);
            public TreeNodeModel WithIcon(string icon) => new TreeNodeModel(Id, Name, icon, ViewPath);
        }

        protected IIntranetType GetIntranetType(NotificationTypeEnum type) => _notificationTypeProvider.Get((int) type);
        protected IIntranetType GetIntranetType(IntranetActivityTypeEnum type) => _activityTypeProvider.Get((int) type);
    }

    [Application("NotificationSettings", "NotificationSettings", "icon-file-cabinet")]
    public class NotificationSettingsApplication : IApplication
    {

    }
}