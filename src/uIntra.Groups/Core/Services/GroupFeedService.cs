using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.CentralFeed;
using uIntra.Core.Caching;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupFeedService : FeedService, IGroupFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IGroupService _groupService;

        public GroupFeedService(
            ICacheService cacheService,
            IFeedTypeProvider centralFeedTypeProvider,
            IEnumerable<IFeedItemService> feedItemServices,
            IGroupActivityService groupActivityService,
            IGroupService groupService)
            : base(feedItemServices, cacheService, centralFeedTypeProvider)
        {
            _feedItemServices = feedItemServices;
            _groupActivityService = groupActivityService;
            _groupService = groupService;
        }

        public IEnumerable<IFeedItem> GetFeed(IIntranetType type, Guid groupId)
        {
            return GetFeed(groupId)
                .Where(i => i.Type.Id == type.Id);
        }

        public IEnumerable<IFeedItem> GetFeed(Guid groupId)
        {
            return _feedItemServices
                .SelectMany(service => service.GetItems())
                .Where(i => IsGroupActivity(groupId, i));
        }

        public IEnumerable<IFeedItem> GetFeed(IIntranetType type, IEnumerable<Guid> groupIds)
        {
            return GetFeed(groupIds)
                .Where(i => i.Type.Id == type.Id);
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
            return assignedGroupId.HasValue && groupIds.Contains(assignedGroupId.Value) && !_groupService.Get(assignedGroupId.Value).IsHidden;
        }

        private bool IsGroupActivity(Guid groupId, IFeedItem item) => IsGroupActivity(groupId.ToEnumerable(), item);
    }
}