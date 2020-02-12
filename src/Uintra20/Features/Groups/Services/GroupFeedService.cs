using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Services
{
    public class GroupFeedService :
        FeedService,
        IGroupFeedService
    {
        private readonly IGroupActivityService _groupActivityService;
        private readonly IGroupService _groupService;
        private readonly IEnumerable<IFeedItemService> _feedItemServices;

        public GroupFeedService(
            IEnumerable<IFeedItemService> feedItemServices,
            ICacheService cacheService,
            IGroupActivityService groupActivityService,
            IGroupService groupService)
            : base(feedItemServices, cacheService)
        {
            _feedItemServices = feedItemServices;
            _groupActivityService = groupActivityService;
            _groupService = groupService;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId)
        {
            var feedItems = GetFeed(groupId);

            return feedItems.Where(i => i.Type.ToInt() == type.ToInt());
        }

        public IEnumerable<IFeedItem> GetFeed(Guid groupId)
        {
            var selectMany = _feedItemServices
                .SelectMany(service => service.GetItems());

            return selectMany.Where(i => IsGroupActivity(groupId, i));
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupIds)
        {
            var feedItems = GetFeed(groupIds);

            return feedItems.Where(i => i.Type.ToInt() == type.ToInt());
        }

        public IEnumerable<IFeedItem> GetFeed(IEnumerable<Guid> groupIds)
        {
            var selectMany = _feedItemServices
                .SelectMany(service => service.GetItems());

            return selectMany.Where(i => IsGroupActivity(groupIds, i));
        }

        private bool IsGroupActivity(IEnumerable<Guid> groupIds, IFeedItem item)
        {
            var assignedGroupId = _groupActivityService.GetGroupId(item.Id);

            return assignedGroupId.HasValue
                   && groupIds.Contains(assignedGroupId.Value)
                   && !_groupService.Get(assignedGroupId.Value).IsHidden;
        }

        private bool IsGroupActivity(Guid groupId, IFeedItem item)
        {
            var groupIds = groupId.ToEnumerableOfOne();
            var isGroupActivity = IsGroupActivity(groupIds, item);

            return isGroupActivity;
        }
    }
}