using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed.Entities;
using uCommunity.Core.Activity;
using uCommunity.Core.Caching;

namespace uCommunity.CentralFeed
{
    public static class CentralFeedConstants
    {
        public const string CentralFeedCacheKey = "CentralFeed";
        public const string CentralFeedSettingsCacheKey = "CentralFeedSettings";
    }

    public class CentralFeedService : ICentralFeedService
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IEnumerable<ICentralFeedItemService> _feedItemServices;

        public CentralFeedService(IMemoryCacheService memoryCacheService, IEnumerable<ICentralFeedItemService> feedItemServices)
        {
            _memoryCacheService = memoryCacheService;
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
            var settings = _memoryCacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetAllSettings, GetCacheExpiration()).Single(feedSettings => feedSettings.Type == type);
            return settings;
        }

        private IEnumerable<ICentralFeedItem> GetAllItems()
        {
            var items = _feedItemServices.SelectMany(service => service.GetItems());
            return items;
        }

        private IEnumerable<CentralFeedSettings> GetAllSettings()
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