using System;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.Groups.Web
{
    public abstract class GroupFeedControllerBase : FeedControllerBase
    {
        protected override string OverviewViewPath => "~/App_Plugins/Groups/Feed/GroupFeedOverviewView.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/Groups/Feed/GroupFeedDetailsView.cshtml";
        protected override string ListViewPath => "~/App_Plugins/Groups/Feed/GroupFeedList.cshtml";
        protected override string NavigationViewPath => "-";
        protected override string LatestActivitiesViewPath => "-";

        public GroupFeedControllerBase(
            ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            ICentralFeedService centralFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            ICentralFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IIntranetUser> intranetUserService) 
            : base(centralFeedContentHelper,
                  subscribeService,
                  centralFeedService,
                  activitiesServiceFactory,
                  intranetUserContentHelper,
                  centralFeedTypeProvider,
                  intranetUserService)
        {
        }


        protected override DetailsViewModel GetDetailsViewModel(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
