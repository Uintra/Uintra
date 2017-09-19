using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Subscribe;

namespace Compent.uIntra.Controllers
{

    public class CentralFeedController : CentralFeedControllerBase
    {
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;
        private readonly IGroupFeedService _groupFeedService;

        public CentralFeedController(
            ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IGroupMember> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IFeedTypeProvider centralFeedTypeProvider,
            ICentralFeedLinkService centralFeedLinkService,
            IGroupFeedService groupFeedService) 
            : base(centralFeedService,
                  centralFeedContentHelper,
                  activitiesServiceFactory,
                  subscribeService,
                  intranetUserService,
                  intranetUserContentHelper,
                  centralFeedTypeProvider,
                  centralFeedLinkService)
        {
            _intranetUserService = intranetUserService;
            _groupFeedService = groupFeedService;
        }

        protected override IEnumerable<IFeedItem> GetCentralFeedItems(IIntranetType type)
        {
            var groupIds = _intranetUserService.GetCurrentUser().GroupIds;

            var groupFeed = IsTypeForAllActivities(type)
                ? _groupFeedService.GetFeed(groupIds)
                : _groupFeedService.GetFeed(type, groupIds);

            return base.GetCentralFeedItems(type)
                .Concat(groupFeed)
                .OrderByDescending(item => item.PublishDate);
        }
    } 
}