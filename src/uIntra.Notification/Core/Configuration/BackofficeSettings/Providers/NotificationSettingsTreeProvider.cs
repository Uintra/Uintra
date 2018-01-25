using System;
using System.Collections.Generic;
using System.Linq;
using BCLExtensions.Trees;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using static BCLExtensions.Trees.TreeExtensions;

namespace uIntra.Notification.Configuration
{
    /// <summary>
    /// Responsible for mapping defined by category provider categories into tree.
    /// To add your custom notification type <see cref="INotificationSettingCategoryProvider"/>
    /// </summary>
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

        public virtual ITree<TreeNodeModel> GetSettingsTree()
        {
            var categories = _categoryProvider
                .GetAvailableCategories()
                .Select(ParseCategory)
                .Select(WithUrlIdentity)
                .Select(PrettifyName);

            var tree = RootNode.WithChildren(categories);

            return tree;
        }

        protected virtual ITree<TreeNodeModel> ParseCategory(NotificationSettingsCategoryDto dto)
        {
            var categoryNode = GetCategoryNode(dto.ActivityType);
            var children = dto.NotificationTypes.Select(GetSettingsNode);

            return categoryNode.WithChildren(children);
        }

        protected virtual ITree<TreeNodeModel> GetCategoryNode(Enum activityType) =>
            GetNode(activityType.ToInt(), activityType.ToString(), CategoryIconAlias, CategoryRoutePath);

        protected virtual ITree<TreeNodeModel> GetSettingsNode(Enum notificationType) =>
            GetNode(notificationType.ToInt(), notificationType.ToString(), SettingsIconAlias, SettingRoutePath);

        protected virtual ITree<TreeNodeModel> RootNode => GetNode("-1", "root", string.Empty, CategoryRoutePath);

        protected ITree<TreeNodeModel> WithUrlIdentity(ITree<TreeNodeModel> tree)
        {
            TreeNodeModel AddNotificationTypeParameter(TreeNodeModel model) =>
                model.WithViewPath(model.ViewPath + "?notificationType=" + model.Id);

            TreeNodeModel AddActivityTypeParameter(TreeNodeModel model, string type) =>
                model.WithViewPath(model.ViewPath + "&activityType=" + type).WithId($"{model.Id}-{type}");

            var mappedTree = tree.Catamorphism(
                (node, children) => GetNode(
                    node.Id,
                    node.Name,
                    node.Icon,
                    node.ViewPath,
                    children.Select(c => Tree(AddActivityTypeParameter(c.Value, node.Id), c.GetChildren()))),
                leaf => Tree(AddNotificationTypeParameter(leaf).WithIcon("icon-navigation-right")));

            return mappedTree.Select(n => n.WithViewPath(n.ViewPath + "&id=" + n.Id));
        }

        protected ITree<TreeNodeModel> GetNode(object id, object name, string icon, string viewPath, params ITree<TreeNodeModel>[] children) =>
            GetNode(id, name, icon, viewPath, children.AsEnumerable());

        protected ITree<TreeNodeModel> GetNode(object id, object name, string icon, string viewPath, IEnumerable<ITree<TreeNodeModel>> children) =>
            Tree(new TreeNodeModel(id.ToString(), name.ToString(), icon, viewPath), children);

        protected string SplitOnUpperCaseLetters(string name) => name.SplitOnUpperCaseLetters();

        protected ITree<TreeNodeModel> PrettifyName(ITree<TreeNodeModel> node) => node.Select(v => v.WithName(v.Name.SplitOnUpperCaseLetters()));
    }
}