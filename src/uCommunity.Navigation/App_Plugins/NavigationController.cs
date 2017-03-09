using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using uCommunity.Core.App_Plugins.Core.Extentions;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.App_Plugins
{
    public class NavigationController : SurfaceController
    {
        private string NavigationLeftSideMenuView => "~/App_Plugins/Navigation/View/LeftSideMenu.cshtml";

        private readonly INavigationModelBuilder _navigationModelBuilder;

        public NavigationController(INavigationModelBuilder navigationModelBuilder)
        {
            _navigationModelBuilder = navigationModelBuilder;
        }

        public ActionResult LeftSideMenu()
        {
            var leftNavigationMenu = _navigationModelBuilder.GetLeftSideMenu();
            var result = leftNavigationMenu.Map<MenuViewModel>();

            return PartialView(NavigationLeftSideMenuView, result);
        }
    }
}
