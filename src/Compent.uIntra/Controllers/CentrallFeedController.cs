using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Activity;
using Compent.Uintra.Core.Feed;
using Uintra.CentralFeed;
using Uintra.CentralFeed.Web;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Feed;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Groups;
using Uintra.Subscribe;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
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
            IFeedLinkService feedLinkService,
            IGroupFeedService groupFeedService,
            IFeedActivityHelper feedActivityHelper,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            IPermissionsService permissionsService,
            UmbracoHelper umbracoHelper,
            IActivityTypeProvider activityTypeProvider,
            IContextTypeProvider contextTypeProvider,
            IFeedFilterService feedFilterService)
            : base(
                  centralFeedService,
                  centralFeedContentService,
                  activitiesServiceFactory,
                  subscribeService,
                  intranetUserService,
                  intranetUserContentProvider,
                  centralFeedTypeProvider,
                  feedLinkService,
                  feedFilterStateService,
                  permissionsService,
                  activityTypeProvider,
                  contextTypeProvider,
                  feedFilterService)
        {
            _intranetUserService = intranetUserService;
            _groupFeedService = groupFeedService;
            _feedActivityHelper = feedActivityHelper;
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