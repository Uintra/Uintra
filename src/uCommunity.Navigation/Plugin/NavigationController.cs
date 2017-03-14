using System.Web.Mvc;
using uCommunity.Core.Extentions;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Content
{
    public class NavigationController : SurfaceController
    {
        private string LeftNavigationView => "~/App_Plugins/Navigation/LeftNavigation/View/LeftSideMenu.cshtml";
        private string SubNavigationView => "~/App_Plugins/Navigation/SubNavigation/View/SubNavigationMenu.cshtml";

        private readonly ILeftSideMenuModelBuilder _leftSideMenuModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;

        public NavigationController(
            ILeftSideMenuModelBuilder leftSideMenuModelBuilder, 
            ISubNavigationModelBuilder subNavigationModelBuilder
            )
        {
            _leftSideMenuModelBuilder = leftSideMenuModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
        }

        public ActionResult LeftSideMenu()
        {
            var leftNavigationMenu = _leftSideMenuModelBuilder.GetMenu();
            var result = leftNavigationMenu.Map<MenuViewModel>();

            return PartialView(LeftNavigationView, result);
        }

        public ActionResult SubNavigationMenu()
        {
            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return PartialView(SubNavigationView, result);
        }
    }
}
