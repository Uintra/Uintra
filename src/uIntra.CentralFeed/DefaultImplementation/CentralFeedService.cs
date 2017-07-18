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
        private readonly ICacheService cacheService;
        private readonly IEnumerable<ICentralFeedItemService> _feedItemServices;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;

        public CentralFeedService(ICacheService cacheService, 
            IEnumerable<ICentralFeedItemService> feedItemServices, 
            ICentralFeedTypeProvider centralFeedTypeProvider)
        {
            this.cacheService = cacheService;
            _feedItemServices = feedItemServices;
            _centralFeedTypeProvider = centralFeedTypeProvider;
        }
        
        public IEnumerable<ICentralFeedItem> GetFeed(IIntranetType type)
        {
            var service = _feedItemServices.Single(s => s.ActivityType.Id == type.Id);
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

        public CentralFeedSettings GetSettings(IIntranetType type)
        {
            var settings = cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration()).Single(feedSettings => feedSettings.Type.Id == type.Id);
            return settings;
        }

        public IEnumerable<CentralFeedSettings> GetAllSettings()
        {
            var settings = cacheService.GetOrSet(CentralFeedConstants.CentralFeedSettingsCacheKey, GetFeedItemServicesSettings, GetCacheExpiration());
            return settings;
        }

        public LatestActivitiesModel GetLatestActivities(LatestActivitiesPanelModel panelModel)
        {
            var activitiesType = _centralFeedTypeProvider.Get(panelModel.TypeOfActivities);
            var latestActivities = GetFeed(activitiesType).Take(panelModel.NumberOfActivities);

            return new LatestActivitiesModel()
            {
                Title = panelModel.Title,
                Teaser = panelModel.Teaser,
                Items = latestActivities
            };
        }

        protected virtual CentralFeedSettings GetDefaultTabSetting()
        {
            return new CentralFeedSettings
            {
                Type = _centralFeedTypeProvider.Get(CentralFeedTypeEnum.All.ToInt()),
                HasSubscribersFilter = false,
                HasPinnedFilter = true
            };
        }

        private IEnumerable<ICentralFeedItem> GetAllItems()
        {
            var items = _feedItemServices.SelectMany(service => service.GetItems());
            return items;
        }

        private IEnumerable<CentralFeedSettings> GetFeedItemServicesSettings()
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