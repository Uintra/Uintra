using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Web;
using uIntra.Navigation;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class GroupController : GroupControllerBase
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public GroupController(
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupLinkProvider groupLinkProvider,
            IUserService userService,
            IGroupMediaService groupMediaService,
            IIntranetUserService<IGroupMember> intranetUserService, 
            IProfileLinkProvider profileLinkProvider,
            UmbracoHelper umbracoHelper, 
            IDocumentTypeAliasProvider documentTypeAliasProvider)
            : base(groupService, groupMemberService, mediaHelper, groupMediaService, intranetUserService, profileLinkProvider, groupLinkProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public override ActionResult LeftNavigation()
        {
            var groupPageXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage());
            var groupPage = _umbracoHelper.TypedContentSingleAtXPath(groupPageXpath);

            var menuItems = groupPage.Children
                .Where(child => child.IsShowPageInSubNavigation())
                .Select(child => new GroupLeftNavigationItemViewModel
                {
                    Name = child.GetNavigationName(),
                    Url = child.Url
                });

            var result = new GroupLeftNavigationMenuViewModel
            {
                Items = menuItems,
                GroupOverviewPageUrl = groupPage.Url,
                IsActive = _umbracoHelper.AssignedContentItem.IsDescendantOrSelf(groupPage)
            };

            return PartialView(LeftNavigationPath, result);
        }
    }
}