using System.Collections.Generic;
using System.Linq;
using uIntra.Core;
using uIntra.Core.Configuration;
using uIntra.Navigation.Configuration;
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

            var contentUnderHeading = CurrentPage.AncestorsOrSelf().SingleOrDefault(pc => pc.Parent != null && pc.Parent.IsHeading());

            var subMenuStartPage = contentUnderHeading ?? CurrentPage.AncestorsOrSelf().SingleOrDefault(pc => pc.Parent != null && IsHomePage(pc.Parent));
            if (subMenuStartPage == null)
            {
                return null;
            }

            var model = new SubNavigationMenuModel
            {
                Rows = GetSubNavigationMenuRows(subMenuStartPage),
                Parent = IsHomePage(CurrentPage.Parent) || IsContentUnavailable(CurrentPage.Parent)
                    ? null
                    : MapToMenuItemModel(CurrentPage.Parent),
                Title = GetNavigationName(subMenuStartPage),
                IsTitleHidden = IsContentPage(subMenuStartPage)
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

        protected virtual IEnumerable<SubNavigationMenuRowModel> GetSubNavigationMenuRows(IPublishedContent subMenuStartPage)
        {
            var selectedItems = CurrentPage.AncestorsOrSelf().Where(pc => !pc.IsHeading() && !IsHomePage(pc)).ToList();

            var menuRows = selectedItems
                .Select(selectedItem => GetContentForSubNavigation(selectedItem).Select(MapToSubNavigationMenuItemModel))
                .Select(menuItems => new SubNavigationMenuRowModel
                {
                    Items = menuItems.ToList()
                })
                .ToList();

            menuRows.Reverse();

            var topLevelMenuRow = menuRows.First();
            topLevelMenuRow.Items.Insert(0, MapToSubNavigationMenuItemModel(subMenuStartPage));

            return menuRows;
        }

        protected virtual MenuItemModel MapToMenuItemModel(IPublishedContent publishedContent)
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

        protected virtual SubNavigationMenuItemModel MapToSubNavigationMenuItemModel(IPublishedContent publishedContent)
        {
            var result = new SubNavigationMenuItemModel
            {
                Id = publishedContent.Id,
                Name = GetNavigationName(publishedContent),
                Url = publishedContent.Url,
                IsActive = publishedContent.IsAncestorOrSelf(CurrentPage),
                IsSelected = publishedContent == CurrentPage
            };

            return result;
        }
    }
}
