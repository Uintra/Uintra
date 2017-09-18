using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core.Caching;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupFeedService : FeedService, IGroupFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;

        public GroupFeedService(
            ICacheService cacheService,
            IFeedTypeProvider centralFeedTypeProvider, IEnumerable<IFeedItemService> feedItemServices)
            : base(feedItemServices, cacheService, centralFeedTypeProvider)
        {
            _feedItemServices = feedItemServices;
        }

        public IEnumerable<IFeedItem> GetFeed(IIntranetType type, Guid groupId) =>
            GetFeed(groupId)
                .Where(i => i.Type.Id == type.Id);

        public IEnumerable<IFeedItem> GetFeed(Guid groupId) =>
            _feedItemServices
                .SelectMany(service => service.GetItems())
                .Where(i => IsGroupActivity(groupId, i));

        private bool IsGroupActivity(Guid groupId, IFeedItem item) =>
            item is IGroupActivity activity && activity.GroupId.GetValueOrDefault() == groupId;        
    }
}