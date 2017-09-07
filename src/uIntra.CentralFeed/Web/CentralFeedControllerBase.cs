using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : FeedControllerBase
    {
        protected override string OverviewViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected override string ListViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedList.cshtml";
        protected override string NavigationViewPath => "~/App_Plugins/CentralFeed/View/Navigation.cshtml";
        protected override string LatestActivitiesViewPath => "~/App_Plugins/LatestActivities/View/LatestActivities.cshtml";

        protected CentralFeedControllerBase(
            ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            ICentralFeedTypeProvider centralFeedTypeProvider)
            : base(centralFeedContentHelper, subscribeService, centralFeedService, activitiesServiceFactory, intranetUserContentHelper, centralFeedTypeProvider, intranetUserService)
        {}

        [HttpGet]
        public ActionResult Chicken()
        {
            return Json("I love beakon!", JsonRequestBehavior.AllowGet);
        }
    }
}