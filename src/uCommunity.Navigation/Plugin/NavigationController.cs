using System.Web.Mvc;
using uCommunity.Core.Extentions;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Plugin
{
    public class NavigationController : SurfaceController
    {
        private readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder, 
            ISubNavigationModelBuilder subNavigationModelBuilder, 
            ITopNavigationModelBuilder topNavigationModelBuilder)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
        }

        public ActionResult LeftSideMenu()
        {
            var leftNavigation = _leftSideNavigationModelBuilder.GetMenu();
            var result = leftNavigation.Map<MenuViewModel>();

            return PartialView("~/App_Plugins/Navigation/LeftNavigation/View/Navigation.cshtml", result);
        }

        public ActionResult SubNavigationMenu()
        {
            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return PartialView("~/App_Plugins/Navigation/SubNavigation/View/Navigation.cshtml", result);
        }

        public ActionResult TopMenu()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = topNavigation.Map<TopNavigationViewModel>();

            return PartialView("~/App_Plugins/Navigation/TopNavigation/View/Navigation.cshtml", result);
        }
    }
}
