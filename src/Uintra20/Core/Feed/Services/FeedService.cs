using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;
using Uintra20.Features.CentralFeed.Constants;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Feed.Services
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
            var feedItemsList = feedItems.AsList();

            return feedItemsList.IsEmpty()
                ? default
                : feedItemsList.Max(item => item.ModifyDate).Ticks;
        }

        public FeedSettings GetSettings(Enum type)
        {
            var settings = _cacheService.GetOrSet(
                CentralFeedConstants.CentralFeedSettingsCacheKey,
                GetFeedItemServicesSettings,
                GetCacheExpiration()).ToList();


            var r = settings.Single(feedSettings => feedSettings.Type.ToInt() == type.ToInt());
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