using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra.CentralFeed;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;

namespace Uintra.Groups
{
    public class GroupFeedService : FeedService, IGroupFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IGroupService _groupService;

        public GroupFeedService(
            ICacheService cacheService,
            IEnumerable<IFeedItemService> feedItemServices,
            IGroupActivityService groupActivityService,
            IGroupService groupService)
            : base(feedItemServices, cacheService)
        {
            _feedItemServices = feedItemServices;
            _groupActivityService = groupActivityService;
            _groupService = groupService;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId) =>
            GetFeed(groupId)
                .Where(i => i.Type.ToInt() == type.ToInt());

        public IEnumerable<IFeedItem> GetFeed(Guid groupId) =>
            _feedItemServices
                .SelectMany(service => service.GetItems())
                .Where(i => IsGroupActivity(groupId, i));

        public IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupIds) =>
            GetFeed(groupIds)
                .Where(i => i.Type.ToInt() == type.ToInt());

        public IEnumerable<IFeedItem> GetFeed(IEnumerable<Guid> groupIds) =>
            _feedItemServices
                .SelectMany(service => service.GetItems())
                .Where(i => IsGroupActivity(groupIds, i));

        private bool IsGroupActivity(IEnumerable<Guid> groupIds, IFeedItem item)
        {
            var assignedGroupId = _groupActivityService.GetGroupId(item.Id);
            return assignedGroupId.HasValue && groupIds.Contains(assignedGroupId.Value) && !_groupService.Get(assignedGroupId.Value).IsHidden;
        }

        private bool IsGroupActivity(Guid groupId, IFeedItem item) =>
            IsGroupActivity(groupId.ToEnumerable(), item);
    }
}