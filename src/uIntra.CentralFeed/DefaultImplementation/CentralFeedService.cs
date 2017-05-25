using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Caching;

namespace uIntra.CentralFeed
{
    public static class CentralFeedConstants
    {
        public const string CentralFeedCacheKey = "CentralFeed";
        public const string CentralFeedSettingsCacheKey = "CentralFeedSettings";
    }

    public class CentralFeedService : ICentralFeedService
    {
        private readonly ICacheService cacheService;
        private readonly IEnumerable<ICentralFeedItemService> _feedItemServices;

        public CentralFeedService(ICacheService cacheService, IEnumerable<ICentralFeedItemService> feedItemServices)
        {
            this.cacheService = cacheService;
            _feedItemServices = feedItemServices;
        }

        public IEnumerable<ICentralFeedItem> GetFeed(IntranetActivityTypeEnum type)
        {
            var service = _feedItemServices.Single(s => s.ActivityType == type);
            return service.GetItems();
        }

        public IEnumerable<ICentralFeedItem> GetFeed()
        {
            return GetAllItems();
        }

        public long GetFeedVersion(IEnumerable<ICentralFeedItem> centralFeedItems)
        {
            if (!centralFeedItems.Any())
            {
                return default(long);
            }

            return centralFeedItems.Max(item => item.ModifyDate).Ticks;
        }

        public CentralFeedSettings GetSettings(IntranetActivityTypeEnum type)
        {
            var settings = cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration()).Single(feedSettings => feedSettings.Type == type);
            return settings;
        }

        public IEnumerable<CentralFeedSettings> GetAllSettings()
        {
            var settings = cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration());
            return settings;
        }

        public bool IsPinActual(ICentralFeedItem item)
        {
            if (!item.IsPinned) return false;

            if (item.EndPinDate.HasValue)
            {
                return DateTime.Compare(item.EndPinDate.Value, DateTime.Now) > 0;
            }

            return true;
        }

        private IEnumerable<ICentralFeedItem> GetAllItems()
        {
            var items = _feedItemServices.SelectMany(service => service.GetItems());
            return items;
        }

        private IEnumerable<CentralFeedSettings> GetFeedItemServicesSettings()
        {
            var settings = _feedItemServices.Select(service => service.GetCentralFeedSettings());
            return settings;
        }

        private static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }
    }
}