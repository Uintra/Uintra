using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Extentions;
using Compent.uIntra.Core.Navigation;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Navigation;
using uIntra.Navigation.SystemLinks;
using uIntra.Navigation.Web;
using uIntra.Notification;
using uIntra.Users;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Controllers
{
    public class NavigationController : NavigationControllerBase
    {
        protected override string SystemLinkTitleNodePropertyAlias { get; } = "linksGroupTitle";
        protected override string SystemLinkNodePropertyAlias { get; } = "links";
        protected override string SystemLinkSortOrderNodePropertyAlias { get; } = "sort";
        protected override string SystemLinksContentXPath { get; } = $"root/{DataFolder.ModelTypeAlias}[@isDoc]/{SystemLinkFolder.ModelTypeAlias}[@isDoc]/{SystemLink.ModelTypeAlias}[@isDoc]";

        private readonly INotificationHelper _notificationHelper;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IUiNotifierService _uiNotifierService;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            INotificationHelper notificationHelper,
            ICentralFeedContentHelper centralFeedContentHelper,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            ISystemLinksService systemLinksService,
            IUiNotifierService uiNotifierService, 
            IIntranetUserService<IntranetUser> intranetUserService) :
            base (leftSideNavigationModelBuilder, subNavigationModelBuilder, topNavigationModelBuilder,  systemLinksModelBuilder)

        {
            _notificationHelper = notificationHelper;
            _centralFeedContentHelper = centralFeedContentHelper;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
        }

        public override ActionResult TopNavigation()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = topNavigation.Map<TopMenuViewModel>();
            result.NotificationsUrl = _notificationHelper.GetNotificationListPage().Url;
            result.NotificationList = GetNotificationList();

            return PartialView("~/Views/Navigation/TopNavigation.cshtml", result);
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

        private NotificationListViewModel GetNotificationList() // TODO: Move to base controller
        {
            var totalCount = 0;
            var notificationListPage = _notificationHelper.GetNotificationListPage();
            var itemsCountForPopup = notificationListPage.GetPropertyValue(NotificationConstants.ItemCountForPopupPropertyTypeAlias, default(int));
            var notifications = _uiNotifierService.GetMany(_intranetUserService.GetCurrentUserId(), itemsCountForPopup, out totalCount);
            return new NotificationListViewModel
            {
                Notifications = notifications.Map<IEnumerable<NotificationViewModel>>(),
                BlockScrolling = false
            };
        }
    }
}