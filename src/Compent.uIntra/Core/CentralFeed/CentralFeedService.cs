using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core.Caching;
using uIntra.Core.Extensions;
using uIntra.Groups;

namespace Compent.uIntra.Core.CentralFeed
{
    public class CentralFeedService : FeedService, ICentralFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;

        public CentralFeedService(
            IEnumerable<IFeedItemService> feedItemServices,
            ICacheService cacheService)
            : base(feedItemServices, cacheService)
        {
            _feedItemServices = feedItemServices;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type)
        {
            var service = _feedItemServices.Single(s => s.ActivityType.ToInt() == type.ToInt());
            return service.GetItems().Where(IsCentralFeedActivity);
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            var items = _feedItemServices.SelectMany(service => service.GetItems());
            return items.Where(IsCentralFeedActivity);
        }

        private bool IsCentralFeedActivity(IFeedItem item) =>
            (item as IGroupActivity)?.GroupId == null;
    }
}