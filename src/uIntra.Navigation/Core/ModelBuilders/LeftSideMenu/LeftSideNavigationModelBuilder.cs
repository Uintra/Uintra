using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Configuration;
using uIntra.Core.Exceptions;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Navigation
{
    public class LeftSideNavigationModelBuilder : NavigationModelBuilderBase<MenuModel>, ILeftSideNavigationModelBuilder
    {
        public LeftSideNavigationModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider
            ) : base(umbracoHelper, navigationConfigurationProvider)
        {
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

            return result;
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsHideFromLeftNavigation.Alias);
            return result ?? NavigationConfiguration.IsHideFromLeftNavigation.DefaultValue;
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
                Children = GetHomeSubNavigation(homePage)
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

        private bool IsShowInHomeNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsShowInHomeNavigation.Alias);
            return result ?? NavigationConfiguration.IsShowInHomeNavigation.DefaultValue;
        }

        private IEnumerable<MenuItemModel> BuildLeftMenuTree(IPublishedContent publishedContent, List<int> excludeContentIds)
        {
            if (!publishedContent.Children.Any())
            {
                yield break;
            }

            var publishedContentChildrenItems = GetAvailableContent(publishedContent.Children)
                .Where(pContent => !excludeContentIds.Contains(pContent.Id));

            foreach (var publishedContentChildrenItem in publishedContentChildrenItems)
            {
                var newmenuItem = new MenuItemModel
                {
                    Id = publishedContentChildrenItem.Id,
                    Name = GetNavigationName(publishedContentChildrenItem),
                    Url = publishedContentChildrenItem.Url,
                    Children = BuildLeftMenuTree(publishedContentChildrenItem, excludeContentIds),
                    IsActive = CurrentPage.Id == publishedContentChildrenItem.Id
                };

                yield return newmenuItem;
            }
        }
    }
}
