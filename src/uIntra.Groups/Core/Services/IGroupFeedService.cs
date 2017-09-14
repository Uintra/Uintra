using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core.Caching;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public interface IGroupFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(IIntranetType type, Guid groupId);

        IEnumerable<IFeedItem> GetFeed(Guid groupId);
    }

    public class GroupFeedService : FeedService, IGroupFeedService
    {
        private readonly IEnumerable<IGroupFeedItemService> _groupFeedItemServices;

        public GroupFeedService(
            IEnumerable<IGroupFeedItemService> groupFeedItemServices,
            ICacheService cacheService,
            IFeedTypeProvider centralFeedTypeProvider) 
            : base(groupFeedItemServices, cacheService, centralFeedTypeProvider)
        {
            _groupFeedItemServices = groupFeedItemServices;
        }

        public IEnumerable<IFeedItem> GetFeed(IIntranetType type, Guid groupId) =>
            _groupFeedItemServices
                .Single(s => s.ActivityType.Id == type.Id)
                .GetItems(groupId);

        public IEnumerable<IFeedItem> GetFeed(Guid groupId) =>
            _groupFeedItemServices.SelectMany(service => service.GetItems(groupId));
    }
}