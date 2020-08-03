using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Settings;
using Uintra20.Features.CentralFeed.Constants;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Feed.Services
{
    public abstract class FeedSettingsService : IFeedSettingsService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly ICacheService _cacheService;
        private readonly IPermissionsService _permissionsService;

        protected FeedSettingsService(
	        IEnumerable<IFeedItemService> feedItemServices, 
	        ICacheService cacheService,
	        IPermissionsService permissionsService)
        {
            _feedItemServices = feedItemServices;
            _cacheService = cacheService;
            _permissionsService = permissionsService;
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

        protected IFeedItemService GetFeedItemService(Enum type)
        {
	        return _feedItemServices.Single(service => service.Type.ToInt() == type.ToInt());
        }

        protected bool IsAllowView(Enum type)
        {
	        return _permissionsService.Check((PermissionResourceTypeEnum)type.ToInt(), PermissionActionEnum.View);
        }
    }
}