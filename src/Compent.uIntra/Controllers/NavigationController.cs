using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        protected override string SystemLinksContentXPath { get; } = $"root/{DataFolder.ModelTypeAlias}[@isDoc]/{SystemLinkFolder.ModelTypeAlias}[@isDoc]/{SystemLink.ModelTypeAlias}[@isDoc]";

        private readonly ICentralFeedContentHelper _centralFeedContentHelper;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ICentralFeedContentHelper centralFeedContentHelper,
            ISystemLinksModelBuilder systemLinksModelBuilder) :
            base(leftSideNavigationModelBuilder, subNavigationModelBuilder, topNavigationModelBuilder, systemLinksModelBuilder)

        {
            _centralFeedContentHelper = centralFeedContentHelper;
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
            var currentPageNavigation = currentPage as INavigationComposition;
            while (currentPageNavigation == null && currentPage.Parent != null)
            {
                currentPage = currentPage.Parent;
                currentPageNavigation = currentPage as INavigationComposition;
            }

            if (currentPageNavigation == null)
            {
                return string.Empty;
            }

            var result = " - " + currentPageNavigation.NavigationName;

            while (currentPage.Parent != null && !currentPage.Parent.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias))
            {
                currentPage = currentPage.Parent;
                currentPageNavigation = currentPage as INavigationComposition;
                if (currentPageNavigation != null)
                {
                    result = $" - {currentPageNavigation.NavigationName}{result}";
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

        private static bool IsHomePage(IPublishedContent content)
        {
            return content.DocumentTypeAlias == HomePage.ModelTypeAlias;
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