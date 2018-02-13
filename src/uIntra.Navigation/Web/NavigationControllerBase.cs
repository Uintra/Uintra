using System.Collections.Generic;
using System.Web.Mvc;
using Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
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

        protected virtual string SystemLinkTitleNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkSortOrderNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinksContentXPath { get; } = string.Empty;

        private readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ISystemLinksModelBuilder _systemLinksModelBuilder;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserService _userService;

        protected NavigationControllerBase(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IProfileLinkProvider profileLinkProvider,
            IPermissionsService permissionsService,
            IUserService userService)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
            _intranetUserService = intranetUserService;
            _profileLinkProvider = profileLinkProvider;
            _permissionsService = permissionsService;
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
            var currentUser = _intranetUserService.GetCurrentUser();

            var result = new LeftNavigationUserMenuViewModel
            {
                CurrentUser = currentUser,
                ProfileLink = _profileLinkProvider.GetProfileLink(currentUser.Id)
            };

            return PartialView(LeftNavigationUserMenuViewPath, result);
        }

        public virtual ActionResult UmbracoContentLink()
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            if (_permissionsService.IsUserHasAccessToContent(currentUser, CurrentPage))
            {
                return PartialView(UmbracoContentLinkViewPath, CurrentPage.Id);
            }

            return new EmptyResult();
        }

        public virtual ActionResult GoToUmbracoEditPage(int pageId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var pageUrl = string.Format(NavigationUmbracoConstants.UmbracoEditPageUrl, pageId);

            var umbracoUser = _userService.GetUserById(currentUser.UmbracoId.Value);
            if (umbracoUser == null || umbracoUser.IsLockedOut || !umbracoUser.IsApproved)
            {
                return Redirect(pageUrl);
            }

            UmbracoContext.Security.PerformLogin(umbracoUser.Id);  // back office user always isn't logged in
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
    }
}
