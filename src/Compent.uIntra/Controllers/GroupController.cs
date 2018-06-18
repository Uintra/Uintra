using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Permissions;
using uIntra.Groups.Web;
using uIntra.Navigation;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class GroupController : GroupControllerBase
    {
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupPermissionsService _groupPermissionsService;

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
            IImageHelper imageHelper,
            IGroupPermissionsService groupPermissionsService)
            : base(groupService, groupMemberService, mediaHelper, groupMediaService, intranetUserService, profileLinkProvider, groupLinkProvider, imageHelper)
        {
            _intranetUserService = intranetUserService;
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupPermissionsService = groupPermissionsService;
        }

        public override ActionResult LeftNavigation()
        {
            var groupPageXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage());
            var groupPage = _umbracoHelper.TypedContentSingleAtXPath(groupPageXpath);

            var isPageActive = GetIsPageActiveFunc(_umbracoHelper.AssignedContentItem);

            var menuItems = GetMenuItems(groupPage);

            var result = new GroupLeftNavigationMenuViewModel
            {
                Items = menuItems,
                GroupOverviewPageUrl = groupPage.Url,
                IsActive = isPageActive(groupPage)
            };

            return PartialView(LeftNavigationPath, result);
        }
         
        private IEnumerable<GroupLeftNavigationItemViewModel> GetMenuItems(IPublishedContent rootGroupPage)
        {
            var isPageActive = GetIsPageActiveFunc(_umbracoHelper.AssignedContentItem);
            var role = _intranetUserService.GetCurrentUser().Role;

            var groupPageChildren = rootGroupPage.Children.Where(el => el.IsShowPageInSubNavigation()).ToList();

            foreach (var subPage in groupPageChildren)
            {
                if (subPage.IsShowPageInSubNavigation())
                {
                    if (_groupPermissionsService.ValidatePermission(subPage, role))
                    {
                        yield return MapToLeftNavigationItem(subPage, isPageActive);
                    }
                }
                else
                {
                    yield return MapToLeftNavigationItem(subPage, isPageActive);
                }
            }
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