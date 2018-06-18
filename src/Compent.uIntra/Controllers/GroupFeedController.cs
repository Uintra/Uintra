using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Feed;
using Uintra.CentralFeed;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Feed;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Groups;
using Uintra.Groups.Web;
using Uintra.Subscribe;

namespace Compent.Uintra.Controllers
{
    public class GroupFeedController : GroupFeedControllerBase
    {
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;

        public GroupFeedController(
            ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentProvider intranetUserContentProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IGroupMember> intranetUserService,
            IGroupFeedContentService groupFeedContentContentService,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupMemberService groupMemberService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            IActivityTypeProvider activityTypeProvider,
            IPermissionsService permissionsService,
            IContextTypeProvider contextTypeProvider,
            IFeedLinkService feedLinkService,
            IFeedFilterService feedFilterService)
            : base(
                  subscribeService,
                  groupFeedService,
                  activitiesServiceFactory,
                  intranetUserContentProvider,
                  centralFeedTypeProvider,
                  intranetUserService,
                  groupFeedContentContentService,
                  groupFeedLinkProvider,
                  groupMemberService,
                  feedFilterStateService,
                  permissionsService,
                  contextTypeProvider,
                  feedLinkService,
                  feedFilterService)
        {
            _intranetUserService = intranetUserService;
        }

        protected override FeedListViewModel GetFeedListViewModel(GroupFeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
        {
            var result = base.GetFeedListViewModel(model, filteredItems, centralFeedType);
            var currentUser = _intranetUserService.GetCurrentUser();

            result.IsReadOnly = !currentUser.GroupIds.Contains(model.GroupId);

            return result;
        }

        protected override ActivityFeedOptions GetActivityFeedOptions(Guid id)
        {
            var options = base.GetActivityFeedOptions(id);
            return new ActivityFeedOptionsWithGroups
            {
                Links = options.Links,
                IsReadOnly = options.IsReadOnly,
                GroupInfo = null
            };
        }
    }
}