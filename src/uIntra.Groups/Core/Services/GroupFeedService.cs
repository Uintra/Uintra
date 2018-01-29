using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.CentralFeed;
using uIntra.Core.Caching;

namespace uIntra.Groups
{
    public class GroupFeedService : FeedService, IGroupFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly IGroupActivityService _groupActivityService;

        public GroupFeedService(
            ICacheService cacheService,
            IEnumerable<IFeedItemService> feedItemServices,
            IGroupActivityService groupActivityService)
            : base(feedItemServices, cacheService)
        {
            _feedItemServices = feedItemServices;
            _groupActivityService = groupActivityService;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId)
        {
            return GetFeed(groupId)
                .Where(i => Equals(i.Type, type));
        }

        public IEnumerable<IFeedItem> GetFeed(Guid groupId)
        {
            return _feedItemServices
                .SelectMany(service => service.GetItems())
                .Where(i => IsGroupActivity(groupId, i));
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupIds)
        {
            return GetFeed(groupIds)
                .Where(i => Equals(i.Type, type));  
        }

        public IEnumerable<IFeedItem> GetFeed(IEnumerable<Guid> groupIds)
        {
            return _feedItemServices
                .SelectMany(service => service.GetItems())
                .Where(i => IsGroupActivity(groupIds, i));
        }

        private bool IsGroupActivity(IEnumerable<Guid> groupIds, IFeedItem item)
        {
            var assignedGroupId = _groupActivityService.GetGroupId(item.Id);
            return assignedGroupId.HasValue && groupIds.Contains(assignedGroupId.Value);
        }

        private bool IsGroupActivity(Guid groupId, IFeedItem item)
        {
            return IsGroupActivity(groupId.ToEnumerable(), item);
        }
    }
}