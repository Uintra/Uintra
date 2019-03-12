using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Navigation.SystemLinks;
using Umbraco.Core.Models;
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

        protected NavigationControllerBase(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
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
            return PartialView(BreadcrumbsViewPath, GetBreadcrumbsItems().ToList());
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
            return currentMember.RelatedUser.IsSome
                ? PartialView(UmbracoContentLinkViewPath, CurrentPage.Id)
                : (ActionResult) new EmptyResult();
        }

        public virtual ActionResult GoToUmbracoEditPage(int pageId)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var pageUrl = string.Format(NavigationUmbracoConstants.UmbracoEditPageUrl, pageId);

            currentMember.RelatedUser
                .Filter(user => user.IsValid)
                .IfSome(user => UmbracoContext.Security.PerformLogin(user.Id));

            return Redirect(pageUrl);
        }

        protected virtual IEnumerable<BreadcrumbItemViewModel> GetBreadcrumbsItems()
        {
            var pathToRoot = PathToRoot(CurrentPage).Reverse();
            var result = pathToRoot.Select(page =>
            {
                var navigationName = page.GetNavigationName();
                return new BreadcrumbItemViewModel
                {
                    Name = navigationName.HasValue() ? navigationName : page.Name,
                    Url = page.Url,
                    IsClickable = CurrentPage.Url != page.Url && !page.IsHeading()
                };
            });
            return result;
        }

        protected virtual IEnumerable<IPublishedContent> PathToRoot(IPublishedContent node)
        {
            var current = node;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public virtual ActionResult UserListLink()
        {
            var userListLink = _leftSideNavigationModelBuilder.GetUserListLink();
            var model = userListLink.Map<UserListLinkViewModel>();
            return View(UserListLinkViewPath, model);
        }
    }
}
