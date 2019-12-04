using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.Shared.Extensions;
using Compent.Shared.Extensions.Bcl;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.Settings;
using Uintra20.Core.Feed.State;
using Uintra20.Core.Feed.Web;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed.Web
{
	[RoutePrefix("api/central/feed")]
	public class CentralFeedController : FeedController
	{
		private readonly ICentralFeedService _centralFeedService;
		private readonly IFeedTypeProvider _feedTypeProvider;
		private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
		private readonly IFeedFilterService _centralFeedFilterService;
		private readonly IFeedLinkService _feedLinkService;

		protected virtual int ItemsPerPage => 8;
		public CentralFeedController(
			ICentralFeedService centralFeedService,
			IFeedTypeProvider feedTypeProvider,
			IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
			IFeedFilterService centralFeedFilterService,
			IFeedLinkService feedLinkService) : base(centralFeedService, feedFilterStateService)
		{
			_centralFeedService = centralFeedService;
			_feedTypeProvider = feedTypeProvider;
			_feedFilterStateService = feedFilterStateService;
			_centralFeedFilterService = centralFeedFilterService;
			_feedLinkService = feedLinkService;
		}

		[HttpPost]
		[Route("list")]
		public FeedListViewModel List(FeedListModel model)
		{
			var centralFeedType = _feedTypeProvider[model.TypeId];
			var items = GetCentralFeedItems(centralFeedType).ToList();

			if (IsEmptyFilters(model.FilterState, _feedFilterStateService.CentralFeedCookieExists()))
			{
				model.FilterState = GetFilterStateModel();
			}

			var tabSettings = _centralFeedService.GetSettings(centralFeedType);

			var filteredItems = _centralFeedFilterService.ApplyFilters(items, model.FilterState, tabSettings).ToList();


			var centralFeedModel = GetFeedListViewModel(model, filteredItems, centralFeedType);
			var filterState = MapToFilterState(centralFeedModel.FilterState);
			_feedFilterStateService.SaveFiltersState(filterState);

			return centralFeedModel;
		}


		protected virtual (IEnumerable<IFeedItem> activities, int totalCount) GetLatestActivities(Enum activityType, int activityAmount)
		{
			var items = GetCentralFeedItems(activityType).ToList();
			var filteredItems = FilterLatestActivities(items).Take(activityAmount);
			var sortedItems = Sort(filteredItems, activityType);

			return (sortedItems, items.Count);
		}

		protected virtual FeedListViewModel GetFeedListViewModel(FeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
		{
			var take = model.Page * ItemsPerPage;
			var pagedItemsList = SortForFeed(filteredItems, centralFeedType).Take(take).ToList();

			var settings = _centralFeedService
				.GetAllSettings()
				.AsList();

			var tabSettings = settings
				.Single(s => s.Type.ToInt() == model.TypeId)
				.Map<FeedTabSettings>();

			return new FeedListViewModel
			{
				Version = _centralFeedService.GetFeedVersion(filteredItems),
				Feed = GetFeedItems(pagedItemsList, settings),
				TabSettings = tabSettings,
				Type = centralFeedType,
				BlockScrolling = filteredItems.Count < take,
				FilterState = MapToFilterStateViewModel(model.FilterState)
			};
		}

		protected virtual IEnumerable<IFeedItem> GetCentralFeedItems(Enum type)
		{
			if (IsTypeForAllActivities(type))
			{
				var items = _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate);
				return items;
			}
			return _centralFeedService.GetFeed(type);
		}

		protected virtual IEnumerable<IFeedItem> Sort(IEnumerable<IFeedItem> sortedItems, Enum type)
		{
			IEnumerable<IFeedItem> result;
			switch (type)
			{
				case CentralFeedTypeEnum.All:
					result = sortedItems.OrderBy(i => i, new CentralFeedItemComparer());
					break;
				default:
					result = sortedItems.OrderByDescending(el => el.PublishDate);
					break;
			}
			return result;
		}

		private IEnumerable<IFeedItem> FilterLatestActivities(IEnumerable<IFeedItem> activities)
		{
			var settings = _centralFeedService.GetAllSettings().Where(s => !s.ExcludeFromLatestActivities).Select(s => s.Type);
			var items = activities.Join(settings, item => item.Type.ToInt(), type => type.ToInt(), (item, _) => item);

			return items;
		}


		protected override ActivityFeedOptions GetActivityFeedOptions(Guid activityId)
		{
			return new ActivityFeedOptions()
			{
				Links = _feedLinkService.GetLinks(activityId)
			};
		}
	}
}