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
            _feedItemServices
                .Single(s => s.ActivityType.Id == type.Id)
                .GetItems();

        public IEnumerable<IFeedItem> GetFeed(Guid groupId) =>
            _feedItemServices.SelectMany(service => service.GetItems());
    }
}