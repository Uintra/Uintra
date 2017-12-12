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

        private readonly INotificationSettingCategoryProvider _categoryProvider;

        public NotificationSettingsTreeProvider(INotificationSettingCategoryProvider categoryProvider)
        {
            _categoryProvider = categoryProvider;
        }

        public virtual Tree<TreeNodeModel> GetSettingsTree()
        {
            var categories = _categoryProvider
                .GetAvailableCategories()
                .Select(ParseCategory)
                .Select(WithUrlIdentity)
                .Select(PrettifyName);

            var tree = RootNode.WithChildren(categories);

            return tree;
        }

        protected virtual Tree<TreeNodeModel> ParseCategory(NotificationSettingsCategoryDto dto)
        {
            var categoryNode = GetCategoryNode(dto.ActivityType);
            var children = dto.NotificationTypes.Select(GetSettingsNode);

            return categoryNode.WithChildren(children);
        }

        protected virtual Tree<TreeNodeModel> GetCategoryNode(IIntranetType activityType) => 
            GetNode(activityType.Id, activityType.Name, CategoryIconAlias, CategoryRoutePath);

        protected virtual Tree<TreeNodeModel> GetSettingsNode(IIntranetType notificationType) => 
            GetNode(notificationType.Id, notificationType.Name, SettingsIconAlias, SettingRoutePath);

        protected virtual Tree<TreeNodeModel> RootNode => GetNode("-1", "root", string.Empty, CategoryRoutePath);

        protected Tree<TreeNodeModel> WithUrlIdentity(Tree<TreeNodeModel> tree)
        {
            TreeNodeModel AddNotificationTypeParameter(TreeNodeModel model) =>
                model.WithViewPath(model.ViewPath + "?notificationType=" + model.Id);

            TreeNodeModel AddActivityTypeParameter(TreeNodeModel model, string type) =>
                model.WithViewPath(model.ViewPath + "&activityType=" + type).WithId($"{model.Id}-{type}");

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

        protected string SplitOnUpperCaseLetters(string name) => name.SplitOnUpperCaseLetters();

        protected Tree<TreeNodeModel> PrettifyName(Tree<TreeNodeModel> node) =>
            node.Select(v => v.WithName(v.Name.SplitOnUpperCaseLetters())); 
    }
}