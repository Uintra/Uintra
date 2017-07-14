using System.Web.Mvc;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class FooModel
    {
        public int EventsAmount { get; set; }
        public string DisplayTitle { get; set; }
    }

    public class CentralFeedController : CentralFeedControllerBase
    {
        public CentralFeedController(ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            ISubscribeService subscribeService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,   
            ICentralFeedTypeProvider centralFeedTypeProvider)
            : base(centralFeedService, centralFeedContentHelper, activitiesServiceFactory, subscribeService, intranetUserService, intranetUserContentHelper, centralFeedTypeProvider)
        {
        }

        [NonAction]
        public override ActionResult LatestActivities(LatestActivitiesPanelModel panelModel)
        {
            return base.LatestActivities(panelModel);
        }

        public ActionResult LatestActivities(FooModel panelModel)
        {
            return null;
        }
    } 
}