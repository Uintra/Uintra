using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Subscribe;
using uCommunity.Subscribe.Web;
using uCommunity.Users.Core;

namespace Compent.uIntra.Controllers
{
    public class SubscribeController : SubscribeControllerBase
    {
        public SubscribeController(
            ISubscribeService subscribeService,
            IIntranetUserService<IntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory) :
            base(subscribeService, intranetUserService, activitiesServiceFactory)
        {
        }
    }
}