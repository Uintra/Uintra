using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public static class CentralFeedConstants
    {
        public const string CentralFeedCacheKey = "CentralFeed";
        public const string CentralFeedSettingsCacheKey = "CentralFeedSettings";
    }

    public class CentralFeedService : ICentralFeedService
    {
        private readonly ICacheService _cacheService;
        private readonly IEnumerable<ICentralFeedItemService> _feedItemServices;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;

        public CentralFeedService(ICacheService cacheService, 
            IEnumerable<ICentralFeedItemService> feedItemServices, 
            ICentralFeedTypeProvider centralFeedTypeProvider)
        {
            _cacheService = cacheService;
            _feedItemServices = feedItemServices;
            _centralFeedTypeProvider = centralFeedTypeProvider;
        }
        
        public IEnumerable<IFeedItem> GetFeed(IIntranetType type)
        {
            var service = _feedItemServices.Single(s => s.ActivityType.Id == type.Id);
            return service.GetItems();
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            return GetAllItems();
        }

        public long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems)
        {
            if (!centralFeedItems.Any())
            {
                return default(long);
            }

            return centralFeedItems.Max(item => item.ModifyDate).Ticks;
        }

        public FeedSettings GetSettings(IIntranetType type)
        {
            var settings = _cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration()).Single(feedSettings => feedSettings.Type.Id == type.Id);
            return settings;
        }

        public IEnumerable<FeedSettings> GetAllSettings()
        {
            var settings = _cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration());
            return settings;
        }

        protected virtual FeedSettings GetDefaultTabSetting()
        {
            return new FeedSettings
            {
                Type = _centralFeedTypeProvider.Get(CentralFeedTypeEnum.All.ToInt()),
                HasSubscribersFilter = false,
                HasPinnedFilter = true
            };
        }

        private IEnumerable<IFeedItem> GetAllItems()
        {
            var items = _feedItemServices.SelectMany(service => service.GetItems());
            return items;
        }

        private IEnumerable<FeedSettings> GetFeedItemServicesSettings()
        {
            var settings = _feedItemServices.Select(service => service.GetCentralFeedSettings()).ToList();
            settings.Add(GetDefaultTabSetting());

            return settings;
        }

        private static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }
    }
}