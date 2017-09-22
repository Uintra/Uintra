using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Feed;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
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

        public GroupFeedController(ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IGroupMember> intranetUserService,
            IGroupContentHelper groupContentHelper,
            IGroupFeedLinksProvider groupFeedLinksProvider,
            IGroupFeedLinkService groupFeedLinkService,
            IGroupMemberService groupMemberService) 
            : base(centralFeedContentHelper, subscribeService, groupFeedService, activitiesServiceFactory, intranetUserContentHelper, centralFeedTypeProvider, intranetUserService, groupContentHelper, groupFeedLinksProvider, groupFeedLinkService, groupMemberService)
        {
            this._intranetUserService = intranetUserService;
        }

        protected override FeedListViewModel GetFeedListViewModel(GroupFeedListModel model, List<IFeedItem> filteredItems,
            IIntranetType centralFeedType)
        {
            var result = base.GetFeedListViewModel(model, filteredItems, centralFeedType);
            var currentUser = _intranetUserService.GetCurrentUser();

            result.IsReadOnly = !currentUser.GroupIds.Contains(model.GroupId);

            return result;
        }

        protected override ActivityFeedOptions GetActivityFeedOptions(IFeedItem i)
        {
            var options = base.GetActivityFeedOptions(i);
            return new ActivityFeedOptionsWithGroups()
            {
                Links = options.Links,
                IsReadOnly = options.IsReadOnly,
                GroupInfo = null
            };
        }
    }
}