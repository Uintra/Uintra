using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using uIntra.Core.Activity;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Services;
using umbraco.businesslogic;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Compent.uIntra.Controllers
{
    [Umbraco.Web.Trees.Tree("AdminUtils", "AdminUtilsTree", "AdminUtils")]
    [PluginController("AdminUtils")]
    public class AdminUtilsController : TreeController
    {
        private readonly INotificationSettingsService _notificationSettingsService;

        public AdminUtilsController(INotificationSettingsService notificationSettingsService)
        {
            _notificationSettingsService = notificationSettingsService;
        }


        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var items = _notificationSettingsService.NotificationPolicies.SelectMany(r => CreateTree(r, queryStrings));
            var trees = new TreeNodeCollection();
            trees.AddRange(items);
            return trees;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }

        private IEnumerable<TreeNode> CreateTree((IntranetActivityTypeEnum, IEnumerable<NotificationTypeEnum> ) former, FormDataCollection queryStrings)
        {
            var parentId = Guid.NewGuid().ToString();
            var parentNode =  CreateTreeNode(parentId, string.Empty, queryStrings, former.Item1.ToString(), "icon-truck", true);
            var childs = former.Item2.Select(e => CreateTreeNode(Guid.NewGuid().ToString(), parentId, queryStrings, e.ToString(), "icon-truck", false));
            return parentNode.AsEnumerableOfOne().Concat(childs);
        }
    }

    [Application("AdminUtils", "AdminUtils", "icon-file-cabinet")]
    public class AdminUtilsApplication : IApplication
    {

    }
}
