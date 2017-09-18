using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public static class CentralFeedConstants
    {
        public const string CentralFeedCacheKey = "CentralFeed";
        public const string CentralFeedSettingsCacheKey = "CentralFeedSettings";
    }

    public class CentralFeedService : FeedService, ICentralFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;

        public CentralFeedService(
            IEnumerable<IFeedItemService> feedItemServices,
            ICacheService cacheService,
            IFeedTypeProvider centralFeedTypeProvider,
            IEnumerable<IFeedItemService> feedItemServices2) 
            : base(feedItemServices, cacheService, centralFeedTypeProvider)
        {
            _feedItemServices = feedItemServices2;
        }

        public IEnumerable<IFeedItem> GetFeed(IIntranetType type)
        {
            var service = _feedItemServices.Single(s => s.ActivityType.Id == type.Id);
            return service.GetItems();
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            var items = _feedItemServices.SelectMany(service => service.GetItems());
            return items;
        }
    }
}