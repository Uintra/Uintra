using System.Collections.Generic;
using System.Web.Mvc;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using uIntra.Core.Extentions;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Web
{
    public abstract class NavigationControllerBase : SurfaceController
    {
        protected virtual string LeftNavigationViewPath { get; } = "~/App_Plugins/Navigation/LeftNavigation/View/Navigation.cshtml";
        protected virtual string SubNavigationViewPath { get; } = "~/App_Plugins/Navigation/SubNavigation/View/Navigation.cshtml";
        protected virtual string TopNavigationViewPath { get; } = "~/App_Plugins/Navigation/TopNavigation/View/Navigation.cshtml";
        protected virtual string SystemLinksViewPath { get; } = "~/App_Plugins/Navigation/SystemLinks/View/SystemLinks.cshtml";
        protected virtual string SystemLinkTitleNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkSortOrderNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinksContentXPath { get; } = string.Empty;

        private readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly ISystemLinksModelBuilder _systemLinksModelBuilder;


        protected NavigationControllerBase(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
        }

        public virtual ActionResult LeftNavigation()
        {
            var leftNavigation = _leftSideNavigationModelBuilder.GetMenu();
            var result = leftNavigation.Map<MenuViewModel>();

            return PartialView(LeftNavigationViewPath, result);
        }

        public virtual ActionResult SubNavigation()
        {
            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return PartialView(SubNavigationViewPath, result);
        }

        public virtual ActionResult TopNavigation()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = topNavigation.Map<TopNavigationViewModel>();

            return PartialView(TopNavigationViewPath, result);
        }

        public virtual PartialViewResult SystemLinks()
        {
            var systemLinks = _systemLinksModelBuilder.Get(SystemLinksContentXPath, SystemLinkTitleNodePropertyAlias,
                SystemLinkNodePropertyAlias, SystemLinkSortOrderNodePropertyAlias, x => x.SortOrder);
            var result = systemLinks.Map<List<SystemLinksViewModel>>();

            return PartialView(SystemLinksViewPath, result);
        }
    }
}
