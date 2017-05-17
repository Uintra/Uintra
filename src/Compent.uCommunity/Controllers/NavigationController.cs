using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uCommunity.Core.Extentions;
using Compent.uCommunity.Core.Navigation;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using uCommunity.Navigation.Web;
using uCommunity.Notification.Core.Services;
using uCommunity.Users.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uCommunity.Controllers
{
    public class NavigationController : NavigationControllerBase
    {
        protected override string MyLinkPageTitleNodePropertyAlias { get; } = "navigationName";
        protected override string SystemLinkTitleNodePropertyAlias { get; } = "linksGroupTitle";
        protected override string SystemLinkNodePropertyAlias { get; } = "links";
        protected override string SystemLinkSortOrderNodePropertyAlias { get; } = "sort";
        protected override string SystemLinksContentXPath { get; } = $"root/{DataFolder.ModelTypeAlias}[@isDoc]/{SystemLinkFolder.ModelTypeAlias}[@isDoc]/{SystemLink.ModelTypeAlias}[@isDoc]";

        private readonly INotificationHelper _notificationHelper;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IMyLinksModelBuilder _myLinksModelBuilder;
        private readonly ISystemLinksModelBuilder _systemLinksModelBuilder;
        private readonly IMyLinksService _myLinksService;
        private readonly ISystemLinksService _systemLinksService;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            INotificationHelper notificationHelper,
            ICentralFeedContentHelper centralFeedContentHelper,
            IMyLinksModelBuilder myLinksModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IMyLinksService myLinksService,
            ISystemLinksService systemLinksService,
        IIntranetUserService<IntranetUser> intranetUserService) :
            base (leftSideNavigationModelBuilder, subNavigationModelBuilder, topNavigationModelBuilder, myLinksModelBuilder, systemLinksModelBuilder, myLinksService, intranetUserService)

        {
            _notificationHelper = notificationHelper;
            _centralFeedContentHelper = centralFeedContentHelper;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _myLinksModelBuilder = myLinksModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
            _systemLinksService = systemLinksService;
        }

        public override ActionResult TopNavigation()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = topNavigation.Map<TopMenuViewModel>();
            result.NotificationsUrl = _notificationHelper.GetNotificationListPage().Url;

            return PartialView("~/App_Plugins/Navigation/TopNavigation/View/Navigation.cshtml", result);
        }

        public override ActionResult SubNavigation()
        {
            if (_centralFeedContentHelper.IsCentralFeedPage(CurrentPage))
            {
                var models = _centralFeedContentHelper.GetTabs(CurrentPage).Map<IEnumerable<CentralFeedTabViewModel>>();
                return PartialView("~/App_Plugins/CentralFeed/View/Navigation.cshtml", models);
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

            var result = currentPageNavigation.NavigationName + " -";

            while (currentPage.Parent != null && !currentPage.Parent.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias))
            {
                currentPage = currentPage.Parent;
                currentPageNavigation = currentPage as INavigationComposition;
                if (currentPageNavigation != null)
                {
                    result = $"{currentPageNavigation.NavigationName} - {result}";
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
                //HideInNavigation = !content.IsShowPageInSubNavigation(),
                Url = content.Url,
                IsActive = content.IsAncestorOrSelf(CurrentPage)
            };
        }
    }
}