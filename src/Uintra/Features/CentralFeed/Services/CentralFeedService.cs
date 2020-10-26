using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Services;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Groups;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Groups.Services;
using Uintra.Infrastructure.Caching;

namespace Uintra.Features.CentralFeed.Services
{
    public class CentralFeedService : FeedSettingsService, ICentralFeedService
    {
        private readonly IEnumerable<IFeedItemService> _feedItemServices;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IGroupService _groupService;

        public CentralFeedService(
            IEnumerable<IFeedItemService> feedItemServices,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IPermissionsService permissionsService,
            IGroupService groupService)
            : base(feedItemServices, cacheService, permissionsService)
        {
            _feedItemServices = feedItemServices;
            _intranetMemberService = intranetMemberService;
            _groupService = groupService;
        }

        public IEnumerable<IFeedItem> GetFeed(Enum type)
        {
            if (!IsAllowView(type))
            {
                return Enumerable.Empty<IFeedItem>();
            }

            var items = 
		            GetFeedItemService(type)
		            .GetItems();

            return AdditionalFilters(items);
        }

        public IEnumerable<IFeedItem> GetFeed()
        {
            var items = _feedItemServices
                .Where(service => IsAllowView(service.Type))
                .SelectMany(service => service.GetItems());

            items = AdditionalFilters(items);

            return items;
        }

        private IEnumerable<IFeedItem> AdditionalFilters(IEnumerable<IFeedItem> items)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            return items.Where(x =>
            {
                var groupFeedItem = (IGroupActivity)x;
                return !groupFeedItem.GroupId.HasValue || !_groupService.Get(groupFeedItem.GroupId.Value).IsHidden && currentMember.GroupIds.Any(g => g == groupFeedItem.GroupId.Value);
            });
        }
    }
}