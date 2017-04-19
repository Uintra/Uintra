using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Subscribe;
using uCommunity.Subscribe.Web;

namespace Compent.uCommunity.Controllers
{
    public class SubscribeController : SubscribeControllerBase
    {
        public SubscribeController(
            ISubscribeService subscribeService,
            IIntranetUserService intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory) :
            base(subscribeService, intranetUserService, activitiesServiceFactory)
        {
        }
    }
}