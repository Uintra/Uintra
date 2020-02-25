using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.Groups;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed.Services
{
    public class CentralFeedService : FeedService, ICentralFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly IPermissionsService _permissionsService;

        public CentralFeedService(
            IEnumerable<IFeedItemService> feedItemServices,
            ICacheService cacheService,
            IPermissionsService permissionsService)
            : base(feedItemServices, cacheService)
        {
            _feedItemServices = feedItemServices;
            _permissionsService = permissionsService;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type)
        {
            if (!_permissionsService.Check((PermissionResourceTypeEnum) type.ToInt(), PermissionActionEnum.View))
            {
                return Enumerable.Empty<IFeedItem>();
            }

            var service = _feedItemServices.SingleOrDefault(s => s.Type.ToInt() == type.ToInt());

            return service == null
                ? Enumerable.Empty<IFeedItem>()
                : service.GetItems(); //.Where(IsCentralFeedActivity);
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            var items = _feedItemServices
                .Where(service => _permissionsService.Check((PermissionResourceTypeEnum)service.Type.ToInt(), PermissionActionEnum.View))
                .SelectMany(service => service.GetItems());
            //return items.Where(IsCentralFeedActivity);
            return items;
        }

        private bool IsCentralFeedActivity(IFeedItem item) =>
            (item as IGroupActivity)?.GroupId == null;
    }
}