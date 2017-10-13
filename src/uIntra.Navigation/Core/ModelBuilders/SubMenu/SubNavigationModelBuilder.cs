using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using uIntra.Core;
using uIntra.Core.Configuration;
using uIntra.Navigation.Configuration;
using uIntra.Navigation.Constants;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Navigation
{
    public class SubNavigationModelBuilder : NavigationModelBuilderBase<SubNavigationMenuModel>, ISubNavigationModelBuilder
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public SubNavigationModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public override SubNavigationMenuModel GetMenu()
        {
            if (IsHomePage(CurrentPage) || IsShowInHomeNavigation(CurrentPage))
            {
                return null;
            }

            var model = new SubNavigationMenuModel
            {
                Items = GetContentForSubNavigation(CurrentPage).Select(MapSubNavigationItem),
                Parent = (IsHomePage(CurrentPage.Parent) || IsContentUnavailable(CurrentPage.Parent)) ?
                    null :
                    MapSubNavigationItem(CurrentPage.Parent),
                Title = GetNavigationName(CurrentPage),
                IsTitleHidden = IsContentPage(CurrentPage),
                ShowBreadcrumbs = Convert.ToBoolean(ConfigurationManager.AppSettings[NavigationApplicationSettingsConstants.NavigationShowBreadcrumbs])
            };

            return model;
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsHideFromSubNavigation.Alias);
            return result ?? NavigationConfiguration.IsHideFromSubNavigation.DefaultValue;
        }

        protected virtual IEnumerable<IPublishedContent> GetContentForSubNavigation(IPublishedContent publishedContent)
        {
            var result = (publishedContent.Children.Any() || IsHomePage(publishedContent.Parent))
                ? publishedContent.Children
                : publishedContent.Parent.Children;

            return GetAvailableContent(result);
        }

        protected virtual bool IsHomePage(IPublishedContent content)
        {
            return content.DocumentTypeAlias == NavigationConfiguration.HomePageAlias;
        }

        protected virtual bool IsContentPage(IPublishedContent content)
        {
            return content.DocumentTypeAlias == _documentTypeAliasProvider.GetContentPage();
        }

        protected virtual MenuItemModel MapSubNavigationItem(IPublishedContent publishedContent)
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
