using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Feed;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Feed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Web;
using uIntra.Subscribe;

namespace Compent.uIntra.Controllers
{
    public class GroupFeedController : GroupFeedControllerBase
    {
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;

        public GroupFeedController(ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentProvider intranetUserContentProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IGroupMember> intranetUserService,
            IGroupFeedContentService groupFeedContentContentService,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupFeedLinkService groupFeedLinkService,
            IGroupMemberService groupMemberService,
            IFeedFilterStateService feedFilterStateService) 
            : base(subscribeService, groupFeedService, activitiesServiceFactory, intranetUserContentProvider, centralFeedTypeProvider, intranetUserService, groupFeedContentContentService, groupFeedLinkProvider, groupFeedLinkService, groupMemberService, feedFilterStateService)
        {
            _intranetUserService = intranetUserService;
        }

        protected override FeedListViewModel GetFeedListViewModel(GroupFeedListModel model, List<IFeedItem> filteredItems,
            IIntranetType centralFeedType)
        {
            var result = base.GetFeedListViewModel(model, filteredItems, centralFeedType);
            var currentUser = _intranetUserService.GetCurrentUser();

            result.IsReadOnly = !currentUser.GroupIds.Contains(model.GroupId);

            return result;
        }

        protected override ActivityFeedOptions GetActivityFeedOptions(Guid id)
        {
            var options = base.GetActivityFeedOptions(id);
            return new ActivityFeedOptionsWithGroups()
            {
                Links = options.Links,
                IsReadOnly = options.IsReadOnly,
                GroupInfo = null
            };
        }
    }
}