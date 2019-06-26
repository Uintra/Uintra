using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Feed;
using Uintra.CentralFeed;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Feed;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Web;

namespace Compent.Uintra.Controllers
{
    public class GroupFeedController : GroupFeedControllerBase
    {
        private readonly IIntranetMemberService<IGroupMember> _intranetMemberService;

        public GroupFeedController(
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetMemberService<IGroupMember> intranetMemberService,
            IGroupFeedContentService groupFeedContentContentService,
            IGroupMemberService groupMemberService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            IPermissionsService permissionsService,
            IContextTypeProvider contextTypeProvider,
            IFeedLinkService feedLinkService,
            IFeedFilterService feedFilterService)
            : base(
                  groupFeedService,
                  activitiesServiceFactory,
                  centralFeedTypeProvider,
                  intranetMemberService,
                  groupFeedContentContentService,
                  groupMemberService,
                  feedFilterStateService,
                  permissionsService,
                  contextTypeProvider,
                  feedLinkService,
                  feedFilterService)
        {
            _intranetMemberService = intranetMemberService;
        }

        protected override FeedListViewModel GetFeedListViewModel(GroupFeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
        {
            var result = base.GetFeedListViewModel(model, filteredItems, centralFeedType);
            var currentMember = _intranetMemberService.GetCurrentMember();

            result.IsReadOnly = !currentMember.GroupIds.Contains(model.GroupId);

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