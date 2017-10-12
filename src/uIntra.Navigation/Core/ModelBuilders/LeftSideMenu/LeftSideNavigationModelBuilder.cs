using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.Core.Configuration;
using uIntra.Core.Exceptions;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Navigation
{
    public class LeftSideNavigationModelBuilder : NavigationModelBuilderBase<MenuModel>, ILeftSideNavigationModelBuilder
    {
        private readonly HttpContext _httpContext;

        public LeftSideNavigationModelBuilder(
            HttpContext httpContext,
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _httpContext = httpContext;
        }

        public override MenuModel GetMenu()
        {
            var result = new MenuModel();

            var homePage = GetHomePage();
            if (IsContentUnavailable(homePage))
            {
                return result;
            }

            var homePageMenu = GetHomePageMenuItem(homePage);
            result.MenuItems.Add(homePageMenu);

            var homePageMenuItemsIds = homePageMenu.Children.Select(mItem => mItem.Id).ToList();
            var leftMenuTree = BuildLeftMenuTree(homePage, homePageMenuItemsIds);
            result.MenuItems.AddRange(leftMenuTree);

            FillClickable(result.MenuItems);
            return result;
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsHideFromLeftNavigation.Alias);
            return result ?? NavigationConfiguration.IsHideFromLeftNavigation.DefaultValue;
        }

        protected override bool IsShowInHomeNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsShowInHomeNavigation.Alias);
            return result ?? NavigationConfiguration.IsShowInHomeNavigation.DefaultValue;
        }

        private MenuItemModel GetHomePageMenuItem(IPublishedContent homePage)
        {
            var result = new MenuItemModel
            {
                Id = homePage.Id,
                Name = GetNavigationName(homePage),
                Url = homePage.Url,
                IsActive = homePage.Id == CurrentPage.Id,
                IsHomePage = true,
                Children = GetHomeSubNavigation(homePage).ToList()
            };

            return result;
        }

        private IPublishedContent GetHomePage()
        {
            var homePage = CurrentPage.AncestorOrSelf(NavigationConfiguration.HomePageAlias);
            if (homePage == null)
            {
                throw new InconsistentDataException("Could not find home page!");
            }

            return homePage;
        }

        private IEnumerable<MenuItemModel> GetHomeSubNavigation(IPublishedContent homePage)
        {
            var result = GetAvailableContent(homePage.Children())
                .Where(IsShowInHomeNavigation)
                .Select(pContent => new MenuItemModel
                {
                    Id = pContent.Id,
                    Url = pContent.Url,
                    Name = GetNavigationName(pContent),
                    IsActive = CurrentPage.Id == pContent.Id || CurrentPage.Parent?.Id == pContent.Id
                });

            return result;
        }

        private IEnumerable<MenuItemModel> BuildLeftMenuTree(IPublishedContent parent, IList<int> excludeContentIds)
        {
            if (!parent.Children.Any())
            {
                yield break;
            }

            var children = GetAvailableContent(parent.Children)
                .Where(pContent => !excludeContentIds.Contains(pContent.Id));

            foreach (var child in children)
            {
                var newMenuItem = new MenuItemModel
                {
                    Id = child.Id,
                    Name = GetNavigationName(child),
                    Url = child.Url,
                    Children = BuildLeftMenuTree(child, excludeContentIds).ToList(),
                    IsActive = child.IsAncestorOrSelf(CurrentPage)
                };

                yield return newMenuItem;
            }
        }

        private void FillClickable(List<MenuItemModel> resultMenuItems)
        {
            var activeItem = resultMenuItems.Find(item => item.IsActive);
            if (activeItem != null)
            {
                activeItem.IsClickable = _httpContext.Request.Url.AbsolutePath.Trim('/') != activeItem.Url.Trim('/');
                return;
            }

            var children = resultMenuItems.SelectMany(item => item.Children).ToList();
            if (children.Count > 0)
            {
                FillClickable(children);
            }
        }
    }
}
