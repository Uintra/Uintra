using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class CentralFeedController : CentralFeedControllerBase
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly ISubscribeService _subscribeService;
        private readonly ICentralFeedService _centralFeedService;

        public CentralFeedController(ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            ISubscribeService subscribeService,
            IActivitiesServiceFactory activitiesServiceFactory)
            : base(centralFeedService, centralFeedContentHelper, activitiesServiceFactory,subscribeService,intranetUserService)
        {
            _intranetUserService = intranetUserService;
            _subscribeService = subscribeService;
            _centralFeedService = centralFeedService;
        }        
       
    }
}