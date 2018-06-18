using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.CentralFeed;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Groups;

namespace Compent.Uintra.Core.CentralFeed
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
            var service = _feedItemServices.Single(s => s.Type.ToInt() == type.ToInt());
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