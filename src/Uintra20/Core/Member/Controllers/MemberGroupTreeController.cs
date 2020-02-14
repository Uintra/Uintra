//using System.Linq;
//using System.Net.Http.Formatting;
//using Uintra20.Core.Member.Entities;
//using Uintra20.Core.Member.Services;
//using Umbraco.Web.Models.Trees;
//using Umbraco.Web.Mvc;
//using Umbraco.Web.Trees;

//namespace Uintra20.Core.Member.Controllers
//{

//    [Tree("member", "memberGroups", TreeTitle = "Member Groups", SortOrder = 1)]
//    [PluginController("memberGroups")]
//    public class MemberGroupTreeController : TreeController
//    {
//        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

//        public MemberGroupTreeController(IIntranetMemberService<IntranetMember> intranetMemberService)
//        {
//            _intranetMemberService = intranetMemberService;
//        }
//        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
//        {
//            var memberGroups = Services.MemberGroupService.GetAll();
//            var nodeCollection = new TreeNodeCollection();

//            var groups = memberGroups
//                .OrderBy(i => i.Name)
//                .Select(memberGroup => CreateTreeNode(memberGroup.Id.ToString(), id, queryStrings, memberGroup.Name,
//                    "icon-users", false))
//                .ToArray();

//            nodeCollection.AddRange(groups);
//            return nodeCollection;
//        }

//        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
//        {
//            return new MenuItemCollection();

//            //if (id == "-1")
//            //{
//            //    menu.Items.Add(new CreateChildEntity(Services.TextService.Localize("create", CultureInfo.CurrentCulture)));
//            //    menu.Items.Add(new RefreshNode(Services.TextService.Localize("refreshNode", CultureInfo.CurrentCulture), true));
//            //    return menu;
//            //}
//            //if (_intranetMemberService.GetCurrentMember().IsSuperUser)
//            //    menu.Items.Add<ActionDelete>(Services.TextService.Localize("delete", CultureInfo.CurrentCulture));
//            //return menu;
//        }
//    }
//}