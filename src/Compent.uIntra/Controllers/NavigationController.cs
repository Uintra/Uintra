using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core;
using Compent.uIntra.Core.Constants;
using Compent.uIntra.Core.Extentions;
using uIntra.CentralFeed;
using uIntra.Navigation;
using uIntra.Navigation.SystemLinks;
using uIntra.Navigation.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Controllers
{
    public class NavigationController : NavigationControllerBase
    {
        protected override string TopNavigationViewPath { get; } = "~/Views/Navigation/TopNavigation/Navigation.cshtml";

        protected override string SystemLinkTitleNodePropertyAlias { get; } = "linksGroupTitle";
        protected override string SystemLinkNodePropertyAlias { get; } = "links";
        protected override string SystemLinkSortOrderNodePropertyAlias { get; } = "sort";
        protected override string SystemLinksContentXPath { get; } 

        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;


        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ICentralFeedContentHelper centralFeedContentHelper,
            ISystemLinksModelBuilder systemLinksModelBuilder, IDocumentTypeAliasProvider documentTypeAliasProvider) :
            base(leftSideNavigationModelBuilder, subNavigationModelBuilder, topNavigationModelBuilder, systemLinksModelBuilder)

        {
            _centralFeedContentHelper = centralFeedContentHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;

            SystemLinksContentXPath = $"root/{_documentTypeAliasProvider.GetDataFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLinkFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLink()}[@isDoc]";
        }

        public override ActionResult SubNavigation()
        {
            if (_centralFeedContentHelper.IsCentralFeedPage(CurrentPage))
            {
                return new EmptyResult();
            }

            var model = new SubNavigationMenuViewModel
            {
                Items = GetContentForSubNavigation(CurrentPage).Where(c => c.IsShowPageInSubNavigation()).Select(MapSubNavigationItem).ToList(),
                Parent = IsHomePage(CurrentPage.Parent) ? null : MapSubNavigationItem(CurrentPage.Parent),
                Title = CurrentPage.GetNavigationName()
            };

            return PartialView(SubNavigationViewPath, model);
        }

        public string GetTitle()
        {
            var currentPage = CurrentPage;
            var isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());
            while (!isPageHasNavigation && currentPage.Parent != null)
            {
                currentPage = currentPage.Parent;
                isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());
            }

            if (!isPageHasNavigation)
            {
                return string.Empty;
            }

            var result = currentPage.GetPropertyValue<string>(NavigationPropertiesConstants.NavigationNamePropName) + " -"; 

            while (currentPage.Parent != null && !currentPage.Parent.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()))
            {
                currentPage = currentPage.Parent;
                isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());
                if (isPageHasNavigation)
                {
                    result = $"{currentPage.GetPropertyValue<string>(NavigationPropertiesConstants.NavigationNamePropName)} - {result}";
                }
            }

            return result;
        }

        private IEnumerable<IPublishedContent> GetContentForSubNavigation(IPublishedContent content)
        {
            if (content.Children.Any() || IsHomePage(content.Parent))
            {
                return content.Children;
            }

            return content.Parent.Children;
        }

        private bool IsHomePage(IPublishedContent content)
        {
            return content.DocumentTypeAlias == _documentTypeAliasProvider.GetHomePage();
        }

        private MenuItemViewModel MapSubNavigationItem(IPublishedContent content)
        {
            return new MenuItemViewModel
            {
                Id = content.Id,
                Name = content.GetNavigationName(),
                Url = content.Url,
                IsActive = content.IsAncestorOrSelf(CurrentPage)
            };
        }
    }
}