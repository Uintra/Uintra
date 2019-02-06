using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Uintra.Users.Web
{
    [PluginController("members")]
    [Tree("member", "memberGroups", "Member Groups", ".sprTreeFolder", ".sprTreeFolder_o", true, 1)]
    public class MemberGroupTreeController : TreeController
    {
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            if (id == "-1")
            {
                menu.Items.Add<CreateChildEntity, ActionNew>(Services.TextService.Localize(ActionNew.Instance.Alias, CultureInfo.CurrentCulture));
                menu.Items.Add<RefreshNode, ActionRefresh>(Services.TextService.Localize(ActionRefresh.Instance.Alias, CultureInfo.CurrentCulture), true);
                return menu;
            }

            menu.Items.Add<ActionDelete>(Services.TextService.Localize(ActionDelete.Instance.Alias, CultureInfo.CurrentCulture));
            return menu;
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var memberGroups = Services.MemberGroupService.GetAll();
            var nodeCollection = new TreeNodeCollection();
            foreach (var memberGroup in memberGroups)
            {
                var node = CreateTreeNode(memberGroup.Id.ToString(), id, queryStrings, memberGroup.Name, "icon-users", false);
                nodeCollection.Add(node);
            }
            return nodeCollection;
        }
    }
}
