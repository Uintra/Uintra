using System.Collections.Generic;
using System.Web.Mvc;
using Compent.Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.Permissions;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Navigation.SystemLinks;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace Uintra.Navigation.Web
{
    public abstract class NavigationControllerBase : SurfaceController
    {
        protected virtual string LeftNavigationViewPath { get; } = "~/App_Plugins/Navigation/LeftNavigation/View/Navigation.cshtml";
        protected virtual string SubNavigationViewPath { get; } = "~/App_Plugins/Navigation/SubNavigation/View/Navigation.cshtml";
        protected virtual string TopNavigationViewPath { get; } = "~/App_Plugins/Navigation/TopNavigation/View/Navigation.cshtml";
        protected virtual string SystemLinksViewPath { get; } = "~/App_Plugins/Navigation/SystemLinks/View/SystemLinks.cshtml";
        protected virtual string BreadcrumbsViewPath { get; } = "~/App_Plugins/Navigation/Breadcrumbs.cshtml";
        protected virtual string LeftNavigationUserMenuViewPath { get; } = "~/App_Plugins/Navigation/LeftNavigation/View/UserMenu.cshtml";
        protected virtual string UmbracoContentLinkViewPath { get; } = "~/App_Plugins/Navigation/UmbracoNavigation/View/UmbracoContentLink.cshtml";
        protected virtual string UserListLinkViewPath { get; } = "~/App_Plugins/Navigation/LeftNavigation/View/UserListLink.cshtml";

        protected virtual string SystemLinkTitleNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkSortOrderNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinksContentXPath { get; } = string.Empty;

        private readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ISystemLinksModelBuilder _systemLinksModelBuilder;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IPermissionsService _basePermissionsService;
        private readonly IUserService _userService;

        protected NavigationControllerBase(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            IPermissionsService basePermissionsService,
            IUserService userService)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
            _basePermissionsService = basePermissionsService;
            _userService = userService;
        }

        public virtual ActionResult LeftNavigation()
        {
            var leftNavigation = _leftSideNavigationModelBuilder.GetMenu();
            var result = leftNavigation.Map<MenuViewModel>();

            return PartialView(LeftNavigationViewPath, result);
        }

        public virtual ActionResult SubNavigation()
        {
            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return PartialView(SubNavigationViewPath, result);
        }

        public virtual ActionResult TopNavigation()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = topNavigation.Map<TopNavigationViewModel>();

            return PartialView(TopNavigationViewPath, result);
        }

        public virtual PartialViewResult SystemLinks()
        {
            var systemLinks = _systemLinksModelBuilder.Get(
                SystemLinksContentXPath,
                SystemLinkTitleNodePropertyAlias,
                SystemLinkNodePropertyAlias,
                SystemLinkSortOrderNodePropertyAlias,
                x => x.SortOrder);

            var result = systemLinks.Map<List<SystemLinksViewModel>>();

            return PartialView(SystemLinksViewPath, result);
        }

        public virtual ActionResult Breadcrumbs()
        {
            return PartialView(BreadcrumbsViewPath, GetBreadcrumbsItems());
        }

        public virtual ActionResult LeftNavigationUserMenu()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var result = new LeftNavigationUserMenuViewModel
            {
                CurrentMember = currentMember.Map<MemberViewModel>(),
                ProfileLink = _profileLinkProvider.GetProfileLink(currentMember.Id)
            };

            return PartialView(LeftNavigationUserMenuViewPath, result);
        }

        public virtual ActionResult UmbracoContentLink()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            if (currentMember.RelatedUser != null)
            {
                return PartialView(UmbracoContentLinkViewPath, CurrentPage.Id);
            }

            return new EmptyResult();
        }

        public virtual ActionResult GoToUmbracoEditPage(int pageId)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var pageUrl = string.Format(NavigationUmbracoConstants.UmbracoEditPageUrl, pageId);

            if (currentMember.RelatedUser == null || currentMember.RelatedUser.IsLockedOut || !currentMember.RelatedUser.IsApproved)
            {
                return Redirect(pageUrl);
            }

            UmbracoContext.Security.PerformLogin(currentMember.RelatedUser.Id);  // back office member always isn't logged in
            return Redirect(pageUrl);
        }

        protected virtual List<BreadcrumbItemViewModel> GetBreadcrumbsItems()
        {
            var result = new List<BreadcrumbItemViewModel>();
            var currentPage = CurrentPage;
            while (currentPage != null)
            {
                var navigationName = currentPage.GetNavigationName();

                result.Add(new BreadcrumbItemViewModel
                {
                    Name = navigationName.HasValue() ? navigationName : currentPage.Name,
                    Url = currentPage.Url,
                    IsClickable = CurrentPage.Url != currentPage.Url && !currentPage.IsHeading()
                });

                currentPage = currentPage.Parent;
            }

            result.Reverse();
            return result;
        }

        public virtual ActionResult UserListLink()
        {
            var userListLink = _leftSideNavigationModelBuilder.GetUserListLink();
            var model = userListLink.Map<UserListLinkViewModel>();
            return View(UserListLinkViewPath, model);
        }
    }
}
