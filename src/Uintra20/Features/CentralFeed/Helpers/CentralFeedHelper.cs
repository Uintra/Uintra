using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Extensions;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.Settings;
using Uintra20.Core.Feed.State;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.CentralFeed.Providers;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.CentralFeed.Helpers
{
    public class CentralFeedHelper : ICentralFeedHelper
    {
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ICentralFeedContentProvider _contentProvider;
        private readonly IGroupFeedService _groupFeedService;
        private readonly IFeedFilterService _feedFilterService;

        public CentralFeedHelper(
            IActivitiesServiceFactory activitiesServiceFactory,
            ICentralFeedService centralFeedService,
            IFeedTypeProvider feedTypeProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            IFeedLinkService feedLinkService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            INodeModelService nodeModelService,
            IUBaselineRequestContext requestContext,
            ICentralFeedContentProvider contentProvider,
            IGroupFeedService groupFeedService,
            IFeedFilterService feedFilterService)
        {
            _feedTypeProvider = feedTypeProvider;
            _centralFeedService = centralFeedService;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _feedLinkService = feedLinkService;
            _feedFilterStateService = feedFilterStateService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _nodeModelService = nodeModelService;
            _requestContext = requestContext;
            _contentProvider = contentProvider;
            _groupFeedService = groupFeedService;
            _feedFilterService = feedFilterService;
        }

        public string AvailableActivityTypes()
        {
            return _centralFeedService
                .GetAllSettings()
                .Where(s => !s.ExcludeFromAvailableActivityTypes)
                .Select(s => (Id: s.Type.ToInt(), Name: s.Type.ToString()))
                .Select(a => new { a.Id, a.Name })
                .OrderBy(el => el.Id)
                .ToJson();
        }

        public FeedListViewModel GetFeedListViewModel(FeedListModel model)
        {
            var centralFeedType = _feedTypeProvider[model.TypeId];

            if (IsEmptyFilters(model.FilterState))
            {
                model.FilterState = GetFilterStateModel();
            }

            var items = model.GroupId.HasValue ? GetGroupFeedItems(centralFeedType, model.GroupId.Value).ToList() : GetCentralFeedItems(centralFeedType).ToList();
            if (!items.Any()) return new FeedListViewModel();

            var tabSettings = _groupFeedService.GetSettings(centralFeedType);
            var filteredItems = _feedFilterService.ApplyFilters(items, model.FilterState, tabSettings).ToList();

            var centralFeedModel = CreateFeedList(model, filteredItems, centralFeedType);
            
            return centralFeedModel;
        }

        public LoadableFeedItemModel GetFeedItems(LatestActivitiesPanelModel node)
        {
            var settings = _centralFeedService.GetAllSettings();
            var centralFeedType = _centralFeedTypeProvider[node.ActivityType.Value.Id];

            var latestActivities = GetLatestActivities(centralFeedType, node.CountToDisplay.Value);
            var feedItems = GetFeedItems(latestActivities.activities, settings, false).ToArray();

            return new LoadableFeedItemModel
            {
                IsShowMore = latestActivities.activities.Count() < latestActivities.TotalCount,
                FeedItems = feedItems
            };
        }

        public bool IsCentralFeedPage(IPublishedContent page)
        {
            return IsHomePage(page) || IsSubPage(page);
        }

        private bool IsHomePage(IPublishedContent page) =>
            _contentProvider.GetOverviewPage().Id == page.Id;

        private bool IsSubPage(IPublishedContent page) =>
            _contentProvider.GetRelatedPages().Any(c => c.IsAncestorOrSelf(page));

        private IEnumerable<FeedItemViewModel> GetFeedItems(IEnumerable<IFeedItem> items,
            IEnumerable<FeedSettings> settings, bool isGroupFeed)
        {
            var activitySettings = settings
                .ToDictionary(s => s.Type.ToInt());

            var result = items
                .Select(i => MapFeedItemToViewModel(i, activitySettings, isGroupFeed));

            return result;
        }

        private IEnumerable<IFeedItem> Sort(IEnumerable<IFeedItem> sortedItems, Enum type)
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

        private IEnumerable<IFeedItem> GetCentralFeedItems(Enum centralFeedType)
        {
            return centralFeedType is CentralFeedTypeEnum.All
                ? _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate)
                : _centralFeedService.GetFeed(centralFeedType);
        }

        protected virtual IEnumerable<IFeedItem> GetGroupFeedItems(Enum type, Guid groupId)
        {
            return type is CentralFeedTypeEnum.All
                ? _groupFeedService.GetFeed(groupId).OrderByDescending(item => item.PublishDate)
                : _groupFeedService.GetFeed(type, groupId);
        }

        private bool IsEmptyFilters(FeedFilterStateModel filterState)
        {
            return !_feedFilterStateService.CentralFeedCookieExists() || filterState == null || !IsAnyFilterSet(filterState);
        }

        private bool IsAnyFilterSet(FeedFilterStateModel filterState)
        {
            return filterState.ShowPinned.HasValue
                   || filterState.IncludeBulletin.HasValue
                   || filterState.ShowSubscribed.HasValue;
        }

        private FeedListViewModel CreateFeedList(FeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
        {
            var centralFeedPanel = GetCentralFeedPanel();
            var itemsPerPage = centralFeedPanel.ItemsPerRequest;
            var skip = itemsPerPage * (model.Page - 1);

            var pagedItemsList = SortForFeed(filteredItems, centralFeedType)
                .Skip(skip)
                .Take(itemsPerPage)
                .ToList();

            var settings = _centralFeedService
                .GetAllSettings()
                .AsList();

            var tabSettings = settings
                .Single(s => s.Type.ToInt() == model.TypeId)
                .Map<FeedTabSettings>();

            return new FeedListViewModel
            {
                Version = _centralFeedService.GetFeedVersion(filteredItems),
                Feed = GetFeedItems(pagedItemsList, settings, model.GroupId.HasValue),
                TabSettings = tabSettings,
                Type = centralFeedType,
                BlockScrolling = filteredItems.Count() < itemsPerPage,
                FilterState = MapToFilterStateViewModel(model.FilterState)
            };
        }

        private FeedFilterStateModel GetFilterStateModel()
        {
            var stateModel = _feedFilterStateService.GetFiltersState();

            var result = new FeedFilterStateModel
            {
                ShowPinned = stateModel.PinnedFilterSelected,
                IncludeBulletin = stateModel.BulletinFilterSelected,
                ShowSubscribed = stateModel.SubscriberFilterSelected
            };

            return result;
        }

        private FeedFilterStateViewModel MapToFilterStateViewModel(FeedFilterStateModel model)
        {
            return new FeedFilterStateViewModel
            {
                ShowPinned = model.ShowPinned ?? false,
                IncludeBulletin = model.IncludeBulletin ?? false,
                ShowSubscribed = model.ShowSubscribed ?? false
            };
        }

        private IEnumerable<IFeedItem> SortForFeed(IEnumerable<IFeedItem> items, Enum type)
        {
            var sortedItems = Sort(items, type);

            return sortedItems.OrderByDescending(el => el.IsPinActual)
                .ToArray();
        }

        private CountableLatestActivities GetLatestActivities(Enum activityType,
            int activityAmount)
        {
            var items = GetCentralFeedItems(activityType).ToArray();
            var filteredItems = FilterLatestActivities(items).Take(activityAmount);
            var sortedItems = Sort(filteredItems, activityType);

            return new CountableLatestActivities
            {
                activities = sortedItems,
                TotalCount = items.Length
            };
        }

        private FeedItemViewModel MapFeedItemToViewModel(IFeedItem i, Dictionary<int, FeedSettings> settings, bool isGroupFeed)
        {
            var options = GetActivityFeedOptions(i.Id);

            var activity = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(i.Type).GetPreviewModel(i.Id, !isGroupFeed);

            return new FeedItemViewModel
            {
                Activity = activity,
                Options = options
            };
        }

        private ActivityFeedOptions GetActivityFeedOptions(Guid activityId)
        {
            return new ActivityFeedOptions
            {
                Links = _feedLinkService.GetLinks(activityId)
            };
        }

        private IEnumerable<IFeedItem> FilterLatestActivities(IEnumerable<IFeedItem> activities)
        {
            var settings = _centralFeedService.GetAllSettings()
                .Where(s => !s.ExcludeFromLatestActivities)
                .Select(s => s.Type);

            var items = activities.Join(settings, item => item.Type.ToInt(), type => type.ToInt(), (item, _) => item);

            return items;
        }

        private CentralFeedPanelViewModel GetCentralFeedPanel()
        {
            var homePageModel = _nodeModelService.GetByAlias<Core.HomePage.HomePageModel>("homePage", _requestContext.HomeNode.RootId);
            var homePageViewModel = _nodeModelService.GetViewModel<Core.HomePage.HomePageViewModel>(homePageModel);
            var centralFeedPanel = (CentralFeedPanelViewModel)homePageViewModel.Panels.Value.FirstOrDefault(i => i.ContentTypeAlias.Equals("centralFeedPanel"));
            return centralFeedPanel;
        }
    }
}
