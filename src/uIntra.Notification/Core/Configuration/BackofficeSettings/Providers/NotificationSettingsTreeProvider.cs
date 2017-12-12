using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Configuration
{
    public class NotificationSettingsTreeProvider : INotificationSettingsTreeProvider
    {

        protected virtual string CategoryRoutePath => "NotificationSettings/NotificationSettingsTree/Category/edit";
        protected virtual string SettingRoutePath => "NotificationSettings/NotificationSettingsTree/Settings/edit";

        protected virtual string CategoryIconAlias => "icon-folder-outline";
        protected virtual string SettingsIconAlias => "icon-navigation-right";

        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public NotificationSettingsTreeProvider(IActivityTypeProvider activityTypeProvider, INotificationTypeProvider notificationTypeProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;
        }

        public virtual Tree<TreeNodeModel> GetSettingsTree()
        {
            var bulletinSettings = GetCategoryNode(GetIntranetType(IntranetActivityTypeEnum.Bulletins))
                .WithChildren(
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentAdded)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentEdited)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentReplied)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded))
                );

            var newsSettings = GetCategoryNode(GetIntranetType(IntranetActivityTypeEnum.News))
                .WithChildren(
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentAdded)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentEdited)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentReplied)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded))
                );

            var eventSettings = GetCategoryNode(GetIntranetType(IntranetActivityTypeEnum.Events))
                .WithChildren(
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.EventUpdated)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.EventHided)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.BeforeStart)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentAdded)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentEdited)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.CommentReplied)),
                    GetSettingsNode(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded))
               );

            var categories = new[] {bulletinSettings, newsSettings, eventSettings}.Select(WithUrlIdentity).Select(PrettifyName);

            var tree = RootNode.WithChildren(categories);

            return tree;
        }

        protected virtual Tree<TreeNodeModel> GetCategoryNode(IIntranetType activityType) => 
            GetNode(activityType.Id, activityType.Name, CategoryIconAlias, CategoryIconAlias);

        protected virtual Tree<TreeNodeModel> GetSettingsNode(IIntranetType notificationType) => 
            GetNode(notificationType.Id, notificationType.Name, SettingsIconAlias, SettingRoutePath);

        protected virtual Tree<TreeNodeModel> RootNode => GetNode("-1", "root", string.Empty, CategoryRoutePath);

        protected Tree<TreeNodeModel> WithUrlIdentity(Tree<TreeNodeModel> tree)
        {
            TreeNodeModel AddNotificationTypeParameter(TreeNodeModel model) =>
                model.WithViewPath(model.ViewPath + "?notificationType=" + model.Id);

            TreeNodeModel AddActivityTypeParameter(TreeNodeModel model, string type) =>
                model.WithViewPath(model.ViewPath + "&activityType=" + type).WithId($"{model.Id}{type}");

            var mappedTree = tree.TreeCatamorphism(
                leaf => Node(AddNotificationTypeParameter(leaf).WithIcon("icon-navigation-right")),
                (node, children) => GetNode(node.Id, node.Name, node.Icon, node.ViewPath, children
                    .Select(c => Node(AddActivityTypeParameter(c.Value, node.Id), c.Children.ToArray())).ToArray()));

            return mappedTree.Select(n => n.WithViewPath(n.ViewPath + "&id=" + n.Id));
        }

        protected Tree<TreeNodeModel> GetNode(object id, object name, string icon, string viewPath, params Tree<TreeNodeModel>[] children)
        {
            return new Tree<TreeNodeModel>(new TreeNodeModel(id.ToString(), name.ToString(), icon, viewPath), children);
        }

        protected Tree<TreeNodeModel> Node(TreeNodeModel treeNodeModel, params Tree<TreeNodeModel>[] children)
        {
            return new Tree<TreeNodeModel>(treeNodeModel, children);
        }

        protected IIntranetType GetIntranetType(NotificationTypeEnum type) => _notificationTypeProvider.Get((int)type);
        protected IIntranetType GetIntranetType(IntranetActivityTypeEnum type) => _activityTypeProvider.Get((int)type);
        protected string SplitOnUpperCaseLetters(string name) => name.SplitOnUpperCaseLetters();

        protected Tree<TreeNodeModel> PrettifyName(Tree<TreeNodeModel> node) =>
            node.Select(v => v.WithName(v.Name.SplitOnUpperCaseLetters()));
    }
}