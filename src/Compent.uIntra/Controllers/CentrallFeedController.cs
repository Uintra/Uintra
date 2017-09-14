using System;
using System.Web.Mvc;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace Compent.uIntra.Controllers
{

    public class CentralFeedController : CentralFeedControllerBase
    {
        public CentralFeedController(ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ISubscribeService subscribeService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,   
            IFeedTypeProvider centralFeedTypeProvider)
            : base(centralFeedService,
                  centralFeedContentHelper,
                  activitiesServiceFactory,
                  subscribeService,
                  intranetUserService,
                  intranetUserContentHelper,
                  centralFeedTypeProvider,
                  activitiesServiceFactory)
        {
        }
    } 
}