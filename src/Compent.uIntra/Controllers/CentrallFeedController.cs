using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Activity;
using Compent.uIntra.Core.Feed;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.Feed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Groups;
using uIntra.Subscribe;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{

    public class CentralFeedController : CentralFeedControllerBase
    {
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;
        private readonly IGroupFeedService _groupFeedService;
        private readonly IFeedActivityHelper _feedActivityHelper;

        public CentralFeedController(
            ICentralFeedService centralFeedService,
            ICentralFeedContentService centralFeedContentService,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IGroupMember> intranetUserService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            ICentralFeedLinkService centralFeedLinkService,
            IGroupFeedService groupFeedService,
            IFeedActivityHelper feedActivityHelper,
            IFeedFilterStateService feedFilterStateService,
            IPermissionsService permissionsService,
            UmbracoHelper umbracoHelper,
            IFeedActivityHelper feedActivityHelper1,
            IActivityTypeProvider activityTypeProvider)
            : base(centralFeedService,
                  centralFeedContentService,
                  activitiesServiceFactory,
                  subscribeService,
                  intranetUserService,
                  intranetUserContentProvider,
                  centralFeedTypeProvider,
                  centralFeedLinkService,
                  feedFilterStateService,
                  permissionsService,
                  activityTypeProvider)
        {
            _intranetUserService = intranetUserService;
            _groupFeedService = groupFeedService;
            _feedActivityHelper = feedActivityHelper1;
        }

        protected override IEnumerable<IFeedItem> GetCentralFeedItems(Enum type)
        {
            var groupIds = _intranetUserService.GetCurrentUser().GroupIds;

            var groupFeed = IsTypeForAllActivities(type)
                ? _groupFeedService.GetFeed(groupIds)
                : _groupFeedService.GetFeed(type, groupIds);

            return base.GetCentralFeedItems(type)
                .Concat(groupFeed)
                .OrderByDescending(item => item.PublishDate);
        }

        protected override ActivityFeedOptions GetActivityFeedOptions(Guid activityId)
        {
            var options = base.GetActivityFeedOptions(activityId);

            return new ActivityFeedOptionsWithGroups
            {
                Links = options.Links,
                IsReadOnly = options.IsReadOnly,
                GroupInfo = _feedActivityHelper.GetGroupInfo(activityId)
            };
        }
    }
}