using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed.Services
{
    public class CentralFeedService : FeedSettingsService, ICentralFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IPermissionsService _permissionsService;

        public CentralFeedService(
            IEnumerable<IFeedItemService> feedItemServices,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IGroupMemberService groupMemberService,
            IGroupActivityService groupActivityService,
            IPermissionsService permissionsService)
            : base(feedItemServices, cacheService)
        {
            _feedItemServices = feedItemServices;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
            _groupActivityService = groupActivityService;
            _permissionsService = permissionsService;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type)
        {
            if (!_permissionsService.Check((PermissionResourceTypeEnum)type.ToInt(), PermissionActionEnum.View))
            {
                return Enumerable.Empty<IFeedItem>();
            }

            var service = _feedItemServices.SingleOrDefault(s => s.Type.ToInt() == type.ToInt());

            var items = service == null
                ? Enumerable.Empty<IFeedItem>()
                : service.GetItems(); //.Where(IsCentralFeedActivity);

            return AdditionalFilters(items);
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            var items = _feedItemServices
                .Where(service => _permissionsService.Check((PermissionResourceTypeEnum)service.Type.ToInt(), PermissionActionEnum.View))
                .SelectMany(service => service.GetItems());

            items = AdditionalFilters(items);

            return items;
        }

        private IEnumerable<IFeedItem> AdditionalFilters(IEnumerable<IFeedItem> items)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            return items.Where(x =>
                !((IGroupActivity)x).GroupId.HasValue || currentMember.GroupIds.Any(g => g == ((IGroupActivity)x).GroupId.Value));
        }

        private bool IsCentralFeedActivity(IFeedItem item) =>
            (item as IGroupActivity)?.GroupId == null;

    }
}