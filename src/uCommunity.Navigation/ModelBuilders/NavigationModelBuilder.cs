using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.App_Plugins.Core.Exceptions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation
{
    public class NavigationModelBuilder : INavigationModelBuilder
    {
        private readonly List<string> _excludeList;

        private readonly UmbracoHelper _umbracoHelper;
        private readonly NavigationConfiguration _navigationConfiguration;

        public NavigationModelBuilder(UmbracoHelper umbracoHelper, NavigationConfiguration navigationConfiguration)
        {
            _umbracoHelper = umbracoHelper;
            _navigationConfiguration = navigationConfiguration;

            _excludeList = _navigationConfiguration.Exclude;
        }

        public MenuModel GetLeftSideMenu()
        {
            var result = new MenuModel();

            var homePage = GetHomePage();
            if (!IsContentVisible(homePage))
            {
                return result;
            }

            var homePageMenu = GetHomePageMenuItem(homePage);
            result.MenuItems.Add(homePageMenu);

            var homePageMenuItemsIds = homePageMenu.Children.Select(mItem => mItem.Id).ToList();
            result.MenuItems.AddRange(BuildLeftMenuTree(homePage, homePageMenuItemsIds));

            return result;
        }

        private MenuItemModel GetHomePageMenuItem(IPublishedContent homePage)
        {
            var result = new MenuItemModel
            {
                Id = homePage.Id,
                Name = GetNavigationName(homePage),
                Url = homePage.Url,
                IsActive = homePage.Id == _umbracoHelper.AssignedContentItem.Id,
                IsHomePage = true,
                Children = GetHomeSubNavigation(homePage)
            };

            return result;
        }

        private IPublishedContent GetHomePage()
        {
            var homePage = _umbracoHelper.AssignedContentItem.AncestorOrSelf(_navigationConfiguration.HomePageAlias);
            if (homePage == null)
            {
                throw new InconsistentDataException("Could not find home page!");
            }

            return homePage;
        }

        private IEnumerable<MenuItemModel> GetHomeSubNavigation(IPublishedContent homePage)
        {
            var subNavigation = homePage.Children();
            var result = subNavigation
                .Where(pContent => !_excludeList.Contains(pContent.DocumentTypeAlias))
                .Where(IsContentVisible)
                .Where(IsShowNavigation)
                .Where(IsShowInHomeNavigation)
                .Select(pContent => new MenuItemModel
                {
                    Id = pContent.Id,
                    Url = pContent.Url,
                    Name = GetNavigationName(pContent),
                    IsActive = _umbracoHelper.AssignedContentItem.Id == pContent.Id || _umbracoHelper.AssignedContentItem.Parent?.Id == pContent.Id
                });

            return result;
        }

        private IEnumerable<MenuItemModel> BuildLeftMenuTree(IPublishedContent publishedContent, List<int> excludeContentIds)
        {
            if (!publishedContent.Children.Any())
            {
                yield break;
            }

            var publishedContentChildrenItems = publishedContent.Children
                .Where(IsContentVisible)
                .Where(IsShowNavigation)
                .Where(pContent => !_excludeList.Contains(pContent.DocumentTypeAlias) && !excludeContentIds.Contains(pContent.Id));

            foreach (var publishedContentChildrenItem in publishedContentChildrenItems)
            {
                var newmenuItem = new MenuItemModel
                {
                    Id = publishedContentChildrenItem.Id,
                    Name = GetNavigationName(publishedContentChildrenItem),
                    Url = publishedContentChildrenItem.Url,
                    Children = BuildLeftMenuTree(publishedContentChildrenItem, excludeContentIds),
                    IsActive = _umbracoHelper.AssignedContentItem.Id == publishedContentChildrenItem.Id
                };

                yield return newmenuItem;
            }
        }

        public virtual bool IsShowNavigation(IPublishedContent publishedContent)
        {
            return !IsHideInNavigation(publishedContent);
        }

        public virtual bool IsHideInNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(_navigationConfiguration.IsHideFromNavigationAlias);
            return result ?? _navigationConfiguration.IsHideFromNavigationDefaultValue;
        }

        public virtual bool IsShowInHomeNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(_navigationConfiguration.IsShowInHomeNavigationAlias);
            return result ?? _navigationConfiguration.IsShowInHomeNavigationDefaultValue;
        }

        public virtual bool IsContentVisible(IPublishedContent publishedContent)
        {
            return true;
        }

        public virtual string GetNavigationName(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<string>(_navigationConfiguration.NavigationNameAlias);
            return string.IsNullOrEmpty(result) ? publishedContent.Name : result;
        }
    }
}
