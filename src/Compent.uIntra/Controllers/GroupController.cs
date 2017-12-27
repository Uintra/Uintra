using System;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Web;
using uIntra.Navigation;
using Umbraco.Core.Models;
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
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IImageHelper imageHelper)
            : base(groupService, groupMemberService, mediaHelper, groupMediaService, intranetUserService, profileLinkProvider, groupLinkProvider, imageHelper)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public override ActionResult LeftNavigation()
        {
            var groupPageXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage());
            var groupPage = _umbracoHelper.TypedContentSingleAtXPath(groupPageXpath);

            var isPageActive = GetIsPageActiveFunc(_umbracoHelper.AssignedContentItem);
                
            var menuItems = groupPage.Children
                .Where(child => child.IsShowPageInSubNavigation())
                .Select(p => MapToLeftNavigationItem(p, isPageActive));

            var result = new GroupLeftNavigationMenuViewModel
            {
                Items = menuItems,
                GroupOverviewPageUrl = groupPage.Url,
                IsActive = isPageActive(groupPage)
            };

            return PartialView(LeftNavigationPath, result);
        }

        private static Func<IPublishedContent, bool> GetIsPageActiveFunc(IPublishedContent currentPage)
        {
            return p => currentPage.Id == p.Id;
        }

        private static GroupLeftNavigationItemViewModel MapToLeftNavigationItem(IPublishedContent page, Func<IPublishedContent, bool> isPageActive)
        {
            return new GroupLeftNavigationItemViewModel
            {
                Name = page.GetNavigationName(),
                Url = page.Url,
                IsActive = isPageActive(page)
            };
        }
    }
}