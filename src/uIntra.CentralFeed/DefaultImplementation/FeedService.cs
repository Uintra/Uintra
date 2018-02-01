using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Extensions;

namespace uIntra.CentralFeed
{
    public abstract class FeedService : IFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly ICacheService _cacheService;

        protected FeedService(IEnumerable<IFeedItemService> feedItemServices, ICacheService cacheService)
        {
            _feedItemServices = feedItemServices;
            _cacheService = cacheService;
        }

        private IEnumerable<FeedSettings> GetFeedItemServicesSettings()
        {
            var settings = _feedItemServices.Select(service => service.GetFeedSettings()).ToList();
            settings.Add(GetDefaultTabSetting());            
            return settings;
        }

        public IEnumerable<FeedSettings> GetAllSettings()
        {
            var settings = _cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration());
            return settings;
        }

        public long GetFeedVersion(IEnumerable<IFeedItem> feedItems)
        {
            if (!feedItems.Any())
            {
                return default;
            }

            return feedItems.Max(item => item.ModifyDate).Ticks;
        }

        public FeedSettings GetSettings(Enum type)
        {
            var settings = _cacheService.GetOrSet(
                CentralFeedConstants.CentralFeedSettingsCacheKey,
                GetFeedItemServicesSettings,
                GetCacheExpiration()).ToList();


            var r = settings.Single(feedSettings => Equals(feedSettings.Type.ToInt(), type.ToInt()));
            return r;
        }

        protected virtual FeedSettings GetDefaultTabSetting()
        {
            return new FeedSettings
            {
                Type = CentralFeedTypeEnum.All,
                HasSubscribersFilter = false,
                HasPinnedFilter = true
            };
        }

        protected static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }
    }
}