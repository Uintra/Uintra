using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation
{
    public class SubNavigationModelBuilder : NavigationModelBuilderBase<SubNavigationMenuModel>, ISubNavigationModelBuilder
    {
        private readonly NavigationConfiguration _navigationConfiguration;

        public SubNavigationModelBuilder(
            UmbracoHelper umbracoHelper, 
            NavigationConfiguration navigationConfiguration) : base(umbracoHelper, navigationConfiguration)
        {
            _navigationConfiguration = navigationConfiguration;
        }

        public override SubNavigationMenuModel GetMenu()
        {
            if (IsHomePage(CurrentPage))
            {
                return null;
            }

            var model = new SubNavigationMenuModel
            {
                Items = GetContentForSubNavigation(CurrentPage).Select(MapSubNavigationItem),
                Parent = (IsHomePage(CurrentPage.Parent) || IsContentUnavailable(CurrentPage.Parent)) ?
                    null :
                    MapSubNavigationItem(CurrentPage.Parent),
                Title = GetNavigationName(CurrentPage)
            };

            return model;
        }

        protected override bool IsSpecificContent(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(_navigationConfiguration.IsShowInSubNavigationAlias);
            return result ?? _navigationConfiguration.IsShowInSubNavigationDefaultValue;
        }

        private IEnumerable<IPublishedContent> GetContentForSubNavigation(IPublishedContent publishedContent)
        {
            var result = (publishedContent.Children.Any() || IsHomePage(publishedContent.Parent)) ?
                publishedContent.Children :
                 publishedContent.Parent.Children;

            return GetSpecificContent(result);
        }

        private bool IsHomePage(IPublishedContent content)
        {
            return content.DocumentTypeAlias == _navigationConfiguration.HomePageAlias;
        }

        private MenuItemModel MapSubNavigationItem(IPublishedContent publishedContent)
        {
            var result = new MenuItemModel
            {
                Id = publishedContent.Id,
                Name = GetNavigationName(publishedContent),
                Url = publishedContent.Url,
                IsActive = publishedContent.IsAncestorOrSelf(CurrentPage)
            };

            return result;
        }
    }
}
