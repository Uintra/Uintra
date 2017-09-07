using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.Groups.Web
{
    public abstract class GroupFeedControllerBase : FeedControllerBase
    {
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
    }
}
