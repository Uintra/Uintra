using Localization.Umbraco.Attributes;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Subscribe;
using uIntra.Subscribe.Web;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    [ThreadCulture]
    public class SubscribeController : SubscribeControllerBase
    {
        public SubscribeController(
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory) :
            base(subscribeService, intranetUserService, activitiesServiceFactory)
        {
        }
    }
}