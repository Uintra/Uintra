using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Web
{
    public abstract class NavigationControllerBase : SurfaceController
    {
        protected virtual string LeftNavigationViewPath { get; } = "~/App_Plugins/Navigation/LeftNavigation/View/Navigation.cshtml";
        protected virtual string SubNavigationViewPath { get; } = "~/App_Plugins/Navigation/SubNavigation/View/Navigation.cshtml";
        protected virtual string TopNavigationViewPath { get; } = "~/App_Plugins/Navigation/TopNavigation/View/Navigation.cshtml";
        protected virtual string MyLinksViewPath { get; } = "~/App_Plugins/Navigation/MyLinks/View/MyLinks.cshtml";
        protected virtual string SystemLinksViewPath { get; } = "~/App_Plugins/Navigation/SystemLinks/View/SystemLinks.cshtml";
        protected virtual string MyLinkIconViewPath { get; } = "~/App_Plugins/Navigation/MyLinks/View/MyLinkIcon.cshtml";
        protected virtual string MyLinkPageTitleNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkTitleNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinkUrlNodePropertyAlias { get; } = string.Empty;
        protected virtual string SystemLinksContentXPath { get; } = string.Empty;

        protected readonly ILeftSideNavigationModelBuilder _leftSideNavigationModelBuilder;
        protected readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        protected readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        protected readonly IMyLinksModelBuilder _myLinksModelBuilder;
        protected readonly ISystemLinksModelBuilder _systemLinksModelBuilder;
        protected readonly IMyLinksService _myLinksService;
        protected readonly IIntranetUserService<IIntranetUser> _intranetUserService;


        protected NavigationControllerBase(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            IMyLinksModelBuilder myLinksModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IMyLinksService myLinksService,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _leftSideNavigationModelBuilder = leftSideNavigationModelBuilder;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _topNavigationModelBuilder = topNavigationModelBuilder;
            _myLinksModelBuilder = myLinksModelBuilder;
            _systemLinksModelBuilder = systemLinksModelBuilder;
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
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

        public virtual PartialViewResult MyLinkIcon()
        {
            var myLinks = _myLinksModelBuilder.Get(x => x.Name);
            var result = new MyLinkIconViewModel();
            result.IsLinked = myLinks.MyLinks.Any(x => x.Url == Request.Url.PathAndQuery.TrimEnd('/'));

            return PartialView(MyLinkIconViewPath, result);
        }

        public virtual PartialViewResult MyLinks()
        {
            var pageName = CurrentPage.GetPropertyValue<string>(MyLinkPageTitleNodePropertyAlias);

            var result = GetMyLinksViewModel(pageName, Request.Url.PathAndQuery.TrimEnd('/'));

            return PartialView(MyLinksViewPath, result);
        }

        public virtual PartialViewResult SystemLinks()
        {
            var systemLinks = _systemLinksModelBuilder.Get(SystemLinksContentXPath, SystemLinkTitleNodePropertyAlias, SystemLinkUrlNodePropertyAlias, x => x.SortOrder);
            var result = systemLinks.Map<SystemLinksViewModel>();

            return PartialView(SystemLinksViewPath, result);
        }

        [HttpPost]
        public virtual PartialViewResult AddRemoveMyLink(string pageName)
        {
            var currentPageUrl = Request.UrlReferrer.PathAndQuery.TrimEnd('/');
            var currentUser = _intranetUserService.GetCurrentUser();
            _myLinksService.AddRemove(currentUser.Id, pageName, currentPageUrl);

            var result = GetMyLinksViewModel(pageName, currentPageUrl);

            return PartialView(MyLinksViewPath, result);
        }

        protected virtual MyLinksViewModel GetMyLinksViewModel(string pageName, string url)
        {
            var myLinks = _myLinksModelBuilder.Get(x => x.Name);
            var result = myLinks.Map<MyLinksViewModel>();
            result.PageName = pageName;
            result.IsLinked = myLinks.MyLinks.Any(x => x.Url == url);

            return result;
        }
    }
}
