using System;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : FeedControllerBase
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        protected override string OverviewViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedDetailsView.cshtml";
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
            ICentralFeedTypeProvider centralFeedTypeProvider,
            IActivitiesServiceFactory activitiesServiceFactory1)
            : base(centralFeedContentHelper, subscribeService, centralFeedService, activitiesServiceFactory, intranetUserContentHelper, centralFeedTypeProvider, intranetUserService)
        {
            _centralFeedService = centralFeedService;
            _activitiesServiceFactory = activitiesServiceFactory1;
        }

        protected override DetailsViewModel GetDetailsViewModel(Guid id)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = service.GetCentralFeedLinks(id);

            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);

            var viewModel = new DetailsViewModel()
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return viewModel;
        }
    }
}