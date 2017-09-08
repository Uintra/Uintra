using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : FeedControllerBase
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;
        protected override string OverviewViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected string DetailsViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedDetailsView.cshtml";
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
        {
            _centralFeedService = centralFeedService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _centralFeedTypeProvider = centralFeedTypeProvider;
        }


        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);
            var links = service.GetCentralFeedLinks(id);
            DetailsViewModel viewModel = new DetailsViewModel()
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return PartialView(DetailsViewPath, viewModel);
        }

        #region Just for test

        [HttpGet]
        public ActionResult Potato(Guid id)
        {
            return RedirectToUmbracoPage(1165, $"?id={id}");
        }

        [HttpGet]
        public ActionResult Chicken()
        {
            return Json("I love beakon!", JsonRequestBehavior.AllowGet);
        } 
        #endregion
    }

    public class DetailsViewModel
    {
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
        public CentralFeedSettings Settings { get; set; }
    }
}