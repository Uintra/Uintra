using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.Groups;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed.Services
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
            var service = _feedItemServices.SingleOrDefault(s => s.Type.ToInt() == type.ToInt());

            return service == null
                   ? Enumerable.Empty<IFeedItem>() 
                   : service.GetItems().Where(IsCentralFeedActivity);
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