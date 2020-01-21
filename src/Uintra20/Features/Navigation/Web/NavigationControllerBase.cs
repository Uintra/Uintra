using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Compent.Extensions;
using UBaseline.Core.Controllers;
using Uintra20.Core.Authentication;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Navigation.ModelBuilders.LeftSideMenu;
using Uintra20.Features.Navigation.ModelBuilders.SubMenu;
using Uintra20.Features.Navigation.ModelBuilders.SystemLinks;
using Uintra20.Features.Navigation.ModelBuilders.TopMenu;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Navigation.Web
{
    public class NavigationControllerBase : UBaselineApiController
    {
        protected virtual string SystemLinkTitleNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkSortOrderNodePropertyAlias { get; } = string.Empty;
        protected virtual IEnumerable<string> SystemLinksContentAliasPath { get; } = Enumerable.Empty<string>();

        protected virtual string DefaultRedirectUrl { get; } = string.Empty;
        protected virtual string UmbracoRedirectUrl { get; } = string.Empty;

        private readonly IAuthenticationService _authenticationService;
        private readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ISystemLinksModelBuilder _systemLinksModelBuilder;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly INavigationModelsBuilder _navigationModelsBuilder;
        private readonly UmbracoContext _umbracoContext;

        protected NavigationControllerBase(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            INavigationModelsBuilder navigationModelsBuilder,
            UmbracoContext umbracoContext)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
            _umbracoContext = umbracoContext;
            _navigationModelsBuilder = navigationModelsBuilder;
        }

        public virtual MenuViewModel LeftNavigation()
        {
            var leftNavigation = _leftSideNavigationModelBuilder.GetMenu();
            var result = leftNavigation.Map<MenuViewModel>();

            return result;
        }

        public virtual SubNavigationMenuViewModel SubNavigation()
        {
            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return result;
        }

        public virtual TopNavigationViewModel TopNavigation()
        {
            var model = _navigationModelsBuilder.GetTopNavigationModel();
            var viewModel = model.Map<TopNavigationViewModel>();
            return viewModel;
        }

        public virtual IEnumerable<SystemLinksViewModel> SystemLinks()
        {
            var systemLinks = _systemLinksModelBuilder.Get(
                SystemLinksContentAliasPath,
                SystemLinkTitleNodePropertyAlias,
                SystemLinkNodePropertyAlias,
                SystemLinkSortOrderNodePropertyAlias,
                x => x.SortOrder);

            var result = systemLinks.Map<List<SystemLinksViewModel>>();

            return result;
        }

        public virtual IEnumerable<BreadcrumbItemViewModel> Breadcrumbs()
        {
            return GetBreadcrumbsItems();
        }

        public virtual LeftNavigationUserMenuViewModel LeftNavigationUserMenu()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var result = new LeftNavigationUserMenuViewModel
            {
                CurrentMember = currentMember.Map<MemberViewModel>(),
                ProfileLink = _profileLinkProvider.GetProfileLink(currentMember.Id)
            };

            return result;
        }

        public virtual ActionResult UmbracoContentLink()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            return currentMember.RelatedUser.IsSome
                ? PartialView(UmbracoContentLinkViewPath, CurrentPage.Id)
                : (ActionResult)new EmptyResult();
        }

        public IHttpActionResult LoginToUmbraco()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var relatedUser = currentMember.RelatedUser;
            if (!relatedUser.IsValid)
                return Redirect(DefaultRedirectUrl);
            _umbracoContext.Security.PerformLogin(relatedUser.Id);
            return Redirect(UmbracoRedirectUrl);
        }

        public IHttpActionResult Logout()
        {
            _authenticationService.Logout();
            return Redirect(DefaultRedirectUrl);
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

        public virtual UserListLinkViewModel UserListLink()
        {
            var userListLink = _leftSideNavigationModelBuilder.GetUserListLink();
            var model = userListLink.Map<UserListLinkViewModel>();
            return model;
        }
    }
}
}