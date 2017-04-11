using System.Web.Mvc;
using Compent.uCommunity.Core.Navigation;
using uCommunity.Core.Extentions;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using uCommunity.Notification.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Plugin
{
    public class NavigationController : SurfaceController
    {
        private readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly INotificationHelper _notificationHelper;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder, 
            ISubNavigationModelBuilder subNavigationModelBuilder, 
            ITopNavigationModelBuilder topNavigationModelBuilder,
            INotificationHelper notificationHelper)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _notificationHelper = notificationHelper;
        }

        public ActionResult LeftNavigation()
        {
            var leftNavigation = _leftSideNavigationModelBuilder.GetMenu();
            var result = leftNavigation.Map<MenuViewModel>();

            return PartialView("~/App_Plugins/Navigation/LeftNavigation/View/Navigation.cshtml", result);
        }

        public ActionResult SubNavigation()
        {
            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return PartialView("~/App_Plugins/Navigation/SubNavigation/View/Navigation.cshtml", result);
        }

        public ActionResult TopNavigation()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = topNavigation.Map<TopMenuViewModel>();
            result.NotificationsUrl = _notificationHelper.GetNotificationListPage().Url;

            return PartialView("~/App_Plugins/Navigation/TopNavigation/View/Navigation.cshtml", result);
        }
    }
}
