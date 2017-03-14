using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.App_Plugins.Core.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation.Core
{
    public class SubNavigationModelBuilder : NavigationModelBuilderBase<SubNavigationMenuModel>, ISubNavigationModelBuilder
    {
        public SubNavigationModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider
            ) : base(umbracoHelper, navigationConfigurationProvider)
        {
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
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsShowInSubNavigation.Alias);
            return result ?? NavigationConfiguration.IsShowInSubNavigation.DefaultValue;
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
            return content.DocumentTypeAlias == NavigationConfiguration.HomePageAlias;
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
