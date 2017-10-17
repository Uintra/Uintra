using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public abstract class FeedService : IFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly ICacheService _cacheService;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;

        protected FeedService(IEnumerable<IFeedItemService> feedItemServices, ICacheService cacheService, IFeedTypeProvider centralFeedTypeProvider)
        {
            _feedItemServices = feedItemServices;
            _cacheService = cacheService;
            _centralFeedTypeProvider = centralFeedTypeProvider;
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
                return default(long);
            }

            return feedItems.Max(item => item.ModifyDate).Ticks;
        }

        public FeedSettings GetSettings(IIntranetType type)
        {
            var settings = _cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration()).Single(feedSettings => feedSettings.Type.Id == type.Id);
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

        protected static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }
    }
}