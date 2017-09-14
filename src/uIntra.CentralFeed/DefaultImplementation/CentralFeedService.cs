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
        private readonly IEnumerable<ICentralFeedItemService> _centralFeedItemServices;

        public CentralFeedService(IEnumerable<ICentralFeedItemService> centralFeedItemServices, ICacheService cacheService, IFeedTypeProvider centralFeedTypeProvider) 
            : base(centralFeedItemServices, cacheService, centralFeedTypeProvider)
        {
            _centralFeedItemServices = centralFeedItemServices;
        }

        public IEnumerable<IFeedItem> GetFeed(IIntranetType type)
        {
            var service = _centralFeedItemServices.Single(s => s.ActivityType.Id == type.Id);
            return service.GetItems();
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            var items = _centralFeedItemServices.SelectMany(service => service.GetItems());
            return items;
        }
    }
}