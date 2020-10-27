using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Services;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Infrastructure.Caching;

namespace Uintra.Features.Groups.Services
{
	public class GroupFeedService :
		FeedSettingsService,
		IGroupFeedService
	{
		private readonly IEnumerable<IFeedItemService> _feedItemServices;


		public GroupFeedService(
			IEnumerable<IFeedItemService> feedItemServices,
			ICacheService cacheService,
			IPermissionsService permissionsService)
			: base(feedItemServices, cacheService, permissionsService)
		{
			_feedItemServices = feedItemServices;
		}

		public IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId)
		{
			if (!IsAllowView(type))
			{
				return Enumerable.Empty<IFeedItem>();
			}

			var feedItemService = GetFeedItemService(type);

			var feedItems = feedItemService.GetGroupItems(groupId);

			return feedItems;
		}

		public IEnumerable<IFeedItem> GetFeed(Guid groupId)
		{
			return
				_feedItemServices
					.Where(service => IsAllowView(service.Type))
					.Select(fs => fs.GetGroupItems(groupId))
					.SelectMany(i => i);

		}

		public IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupIds)
		{
			if (!IsAllowView(type))
			{
				return Enumerable.Empty<IFeedItem>();
			}

			var feedItemService = GetFeedItemService(type);

			var feedItems = groupIds.Select(feedItemService.GetGroupItems).SelectMany(i => i);

			return feedItems;
		}
	}
}