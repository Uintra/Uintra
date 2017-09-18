using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Web;
using uIntra.Subscribe;

namespace Compent.uIntra.Controllers
{
    public class GroupFeedController : GroupFeedControllerBase
    {
        public GroupFeedController(
            ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IIntranetUser> intranetUserService, 
            IGroupContentHelper groupContentHelper,
            IGroupService groupService,
            IGroupFeedLinksProvider groupFeedLinksProvider) 
            : base(centralFeedContentHelper, subscribeService, groupFeedService, activitiesServiceFactory, intranetUserContentHelper, centralFeedTypeProvider, intranetUserService, groupContentHelper, groupService, groupFeedLinksProvider)
        {
        }
    }
}