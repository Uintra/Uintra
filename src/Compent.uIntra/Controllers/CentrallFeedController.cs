using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class CentralFeedController : CentralFeedControllerBase
    {
        public CentralFeedController(ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            ISubscribeService subscribeService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider)
            : base(centralFeedService, centralFeedContentHelper, activitiesServiceFactory, subscribeService, intranetUserService, intranetUserContentHelper, activityTypeProvider)
        {
        }
    } 
}