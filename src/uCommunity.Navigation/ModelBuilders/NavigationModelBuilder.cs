using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation
{
    public class NavigationModelBuilder : INavigationModelBuilder
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly INavigationSettings _navigationSettings;

        public NavigationModelBuilder(UmbracoHelper umbracoHelper, INavigationSettings navigationSettings)
        {
            _umbracoHelper = umbracoHelper;
            _navigationSettings = navigationSettings;
        }

        public MenuViewModel GetLeftSideMenu()
        {
            var result = new MenuViewModel();

            var homePage = GetHomePage();
            if (!IsContentVisible(homePage))
            {
                return result;
            }

            var items = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Id = homePage.Id,
                    Name = GetNavigationName(homePage),
                    Url = homePage.Url,
                    IsActive = homePage.Id == _umbracoHelper.AssignedContentItem.Id,
                    IsHomePage = true,
                    Children = GetHomeSubNavigation(homePage)
                }
            };

            items.AddRange(BuildLeftMenuTree(homePage));

            result.MenuItems = items;
            return result;
        }

        private IPublishedContent GetHomePage()
        {
            var homePage = _umbracoHelper.AssignedContentItem.AncestorOrSelf(_navigationSettings.HomePageAlias);
            if (homePage == null)
            {
                //TODO: InconsistentDataException - common ?
                throw new Exception("Can't find root node!");
            }

            return homePage;
        }

        //private readonly string[] HomePageSubNavigationPagesAliases = { EventsOverview.ModelTypeAlias, NewsOverview.ModelTypeAlias, IdeasOverview.ModelTypeAlias };
        private IEnumerable<MenuItemViewModel> GetHomeSubNavigation(IPublishedContent homePage)
        {
            var subNavigation = homePage.Children(); // HomePageSubNavigationPagesAliases
            var result = subNavigation
                .Where(IsContentVisible)
                .Where(IsShowNavigation)
                .Where(IsShowInHomeNavigation)
                .Select(pContent => new MenuItemViewModel
                {
                    Id = pContent.Id,
                    Url = pContent.Url,
                    Name = GetNavigationName(pContent),
                    // TODO: HideInNavigation ?
                    //HideInNavigation = IsHideInNavigation(pContent),
                    IsActive = _umbracoHelper.AssignedContentItem.Id == pContent.Id || _umbracoHelper.AssignedContentItem.Parent?.Id == pContent.Id
                });

            return result;
        }

        private IEnumerable<MenuItemViewModel> BuildLeftMenuTree(IPublishedContent publishedContent)
        {
            if (!publishedContent.Children.Any())
            {
                yield break;
            }

            var publishedContentChildrenItems = publishedContent.Children
                .Where(IsContentVisible)
                .Where(IsShowNavigation);

            var excludeList = _navigationSettings.Exclude;
            foreach (var publishedContentChildrenItem in publishedContentChildrenItems)
            {
                if (excludeList.Contains(publishedContentChildrenItem.DocumentTypeAlias))
                {
                    continue;
                }

                var newmenuItem = new MenuItemViewModel
                {
                    Id = publishedContentChildrenItem.Id,
                    Name = GetNavigationName(publishedContentChildrenItem),
                    Url = publishedContentChildrenItem.Url,
                    // TODO: HideInNavigation ?
                    //HideInNavigation = publishedContentChildrenItem.GetHideInNavigation(), 
                    Children = BuildLeftMenuTree(publishedContentChildrenItem),
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
            var result = publishedContent.GetPropertyValue<bool?>(_navigationSettings.IsHideInNavigationAlias);
            return result ?? _navigationSettings.IsHideInNavigationDefaultValue;
        }

        public virtual bool IsShowInHomeNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(_navigationSettings.IsShowInHomeNavigationAlias);
            return result ?? _navigationSettings.IsShowInHomeNavigationDefaultValue;
        }

        public virtual bool IsContentVisible(IPublishedContent publishedContent)
        {
            return true;
        }

        public virtual string GetNavigationName(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<string>(_navigationSettings.NavigationNameAlias);
            return string.IsNullOrEmpty(result) ? publishedContent.Name : result;
        }
    }
}
