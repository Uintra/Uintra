using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uCommunity.Core.Navigation;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using uCommunity.Navigation.Web;
using uCommunity.Notification.Core.Services;
using umbraco.NodeFactory;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uCommunity.Controllers
{
    public class NavigationController : NavigationControllerBase
    {
        protected override string PageTitleNodePropertyAlias { get; } = "navigationName";

        private readonly INotificationHelper _notificationHelper;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IMyLinksModelBuilder _myLinksModelBuilder;
        private readonly IMyLinksService _myLinksService;
        private readonly IIntranetUserService _intranetUserService;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            INotificationHelper notificationHelper,
            ICentralFeedContentHelper centralFeedContentHelper,
            IMyLinksModelBuilder myLinksModelBuilder,
            IMyLinksService myLinksService,
            IIntranetUserService intranetUserService) :
            base (leftSideNavigationModelBuilder, subNavigationModelBuilder, topNavigationModelBuilder, myLinksModelBuilder, myLinksService, intranetUserService)

        {
            _notificationHelper = notificationHelper;
            _centralFeedContentHelper = centralFeedContentHelper;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _myLinksModelBuilder = myLinksModelBuilder;
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
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
            //if (_centralFeedContentHelper.IsCentralFeedPage(CurrentPage))
            {
                var models = _centralFeedContentHelper.GetTabs(CurrentPage).Map<IEnumerable<CentralFeedTabViewModel>>();
                return PartialView("~/App_Plugins/CentralFeed/View/Navigation.cshtml", models);
            }

            /*var model = new SubNavigationMenuModel
            {
                Items = GetContentForSubNavigation(CurrentPage).Where(c => c.IsShowPageInSubNavigation()).Select(MapSubNavigationItem),
                Parent = IsHomePage(CurrentPage.Parent) ? null : MapSubNavigationItem(CurrentPage.Parent),
                Title = CurrentPage.GetNavigationName()
            };

            return PartialView("SubNavigationMenu/SubNavigationMenu", model);*/
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
    }
}