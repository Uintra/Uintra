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

        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public NotificationSettingsTreeProvider(IActivityTypeProvider activityTypeProvider, INotificationTypeProvider notificationTypeProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;
        }

        public virtual Tree<TreeNodeModel> GetSettingsTree()
        {
            var icon = "icon-folder-outline";

            var tree = Node("-1", "root", icon, CategoryRoutePath,
                WithUrlIdentity(Node(GetIntranetType(IntranetActivityTypeEnum.Bulletins).Id, IntranetActivityTypeEnum.Bulletins, icon, CategoryRoutePath,
                    Node(GetIntranetType(NotificationTypeEnum.CommentAdded).Id, GetNodeName(NotificationTypeEnum.CommentAdded), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentEdited).Id, GetNodeName(NotificationTypeEnum.CommentEdited), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentReplied).Id, GetNodeName(NotificationTypeEnum.CommentReplied), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded).Id, GetNodeName(NotificationTypeEnum.ActivityLikeAdded), icon, SettingRoutePath))),
                WithUrlIdentity(Node(GetIntranetType(IntranetActivityTypeEnum.News).Id, IntranetActivityTypeEnum.News, icon, CategoryRoutePath,
                    Node(GetIntranetType(NotificationTypeEnum.CommentAdded).Id, GetNodeName(NotificationTypeEnum.CommentAdded), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentEdited).Id, GetNodeName(NotificationTypeEnum.CommentEdited), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentReplied).Id, GetNodeName(NotificationTypeEnum.CommentReplied), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded).Id, GetNodeName(NotificationTypeEnum.ActivityLikeAdded), icon, SettingRoutePath))),
                WithUrlIdentity(Node(GetIntranetType(IntranetActivityTypeEnum.Events).Id, IntranetActivityTypeEnum.Events, icon, CategoryRoutePath,
                    Node(GetIntranetType(NotificationTypeEnum.EventUpdated).Id, GetNodeName(NotificationTypeEnum.EventUpdated), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.EventHided).Id, GetNodeName(NotificationTypeEnum.EventHided), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.BeforeStart).Id, GetNodeName(NotificationTypeEnum.BeforeStart), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentAdded).Id, GetNodeName(NotificationTypeEnum.CommentAdded), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentEdited).Id, GetNodeName(NotificationTypeEnum.CommentEdited), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.CommentReplied).Id, GetNodeName(NotificationTypeEnum.CommentReplied), icon, SettingRoutePath),
                    Node(GetIntranetType(NotificationTypeEnum.ActivityLikeAdded).Id, GetNodeName(NotificationTypeEnum.ActivityLikeAdded), icon, SettingRoutePath))))
                    .Select(n => n.WithViewPath(n.ViewPath + "&id=" + n.Id));

            return tree;
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

        protected IIntranetType GetIntranetType(NotificationTypeEnum type) => _notificationTypeProvider.Get((int)type);
        protected IIntranetType GetIntranetType(IntranetActivityTypeEnum type) => _activityTypeProvider.Get((int)type);
        protected string GetNodeName(NotificationTypeEnum type) => type.ToString().SplitOnUpperCaseLetters();
    }
}