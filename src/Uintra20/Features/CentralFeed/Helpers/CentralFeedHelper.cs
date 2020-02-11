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
        private readonly IFeedFilterService _centralFeedFilterService;
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
            IFeedFilterService centralFeedFilterService,
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
            _centralFeedFilterService = centralFeedFilterService;
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
                .Select(a => new {a.Id, a.Name})
                .OrderBy(el => el.Id)
                .ToJson();
        }

        public FeedListViewModel GetFeedListViewModel(FeedListModel model)
        {
            var centralFeedType = _feedTypeProvider[model.TypeId];
            if (IsEmptyFilters(model.FilterState, _feedFilterStateService.CentralFeedCookieExists()))
            {
                model.FilterState = GetFilterStateModel();
            }

            var items = Enumerable.Empty<IFeedItem>();
            var filteredItems = Enumerable.Empty<IFeedItem>();
            FeedSettings tabSettings;

            if (model.GroupId.HasValue)
            {
                items = GetGroupFeedItems(centralFeedType, model.GroupId.Value);
                if (!items.Any()) return new FeedListViewModel();
                tabSettings = _groupFeedService.GetSettings(centralFeedType);
                filteredItems = _feedFilterService.ApplyFilters(items, model.FilterState, tabSettings);
            }
            else
            {
                items = GetCentralFeedItems(centralFeedType).ToList();
                if (!items.Any()) return new FeedListViewModel();
                tabSettings = _centralFeedService.GetSettings(centralFeedType);
                filteredItems = _centralFeedFilterService.ApplyFilters(items, model.FilterState, tabSettings);
            }

            var centralFeedModel = CreateFeedList(model, filteredItems, centralFeedType);
            var filterState = MapToFilterState(centralFeedModel.FilterState);
            _feedFilterStateService.SaveFiltersState(filterState);

            return centralFeedModel;
        }

        public LoadableFeedItemModel GetFeedItems(LatestActivitiesPanelModel node)
        {
            var settings = _centralFeedService.GetAllSettings();
            var centralFeedType = _centralFeedTypeProvider[node.ActivityType.Value.Id];

            var latestActivities = GetLatestActivities(centralFeedType, node.CountToDisplay.Value);
            var feedItems = GetFeedItems(latestActivities.activities, settings).ToArray();
            
            return new LoadableFeedItemModel
            {
                IsShowMore = latestActivities.activities.Count() < latestActivities.totalCount,
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
            IEnumerable<FeedSettings> settings)
        {
            var activitySettings = settings
                .ToDictionary(s => s.Type.ToInt());

            var result = items
                .Select(i => MapFeedItemToViewModel(i, activitySettings));

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

        private FeedFiltersState MapToFilterState(FeedFilterStateViewModel model)
        {
            return new FeedFiltersState
            {
                PinnedFilterSelected = model.ShowPinned,
                BulletinFilterSelected = model.IncludeBulletin,
                SubscriberFilterSelected = model.ShowSubscribed,
                IsFiltersOpened = _feedFilterStateService.GetFiltersState().IsFiltersOpened
            };
        }

        private bool IsEmptyFilters(FeedFilterStateModel filterState, bool isCookiesExist)
        {
            return !isCookiesExist || filterState == null || !IsAnyFilterSet(filterState);
        }

        private bool IsAnyFilterSet(FeedFilterStateModel filterState)
        {
            return filterState.ShowPinned.HasValue
                   || filterState.IncludeBulletin.HasValue
                   || filterState.ShowSubscribed.HasValue;
        }

        private FeedListViewModel CreateFeedList(
            FeedListModel model, 
            IEnumerable<IFeedItem> filteredItems,
            Enum centralFeedType)
        {
            var homePageModel = _nodeModelService.GetByAlias<Uintra20.Core.HomePage.HomePageModel>("homePage", _requestContext.HomeNode.RootId);
            var homePageViewModel = _nodeModelService.GetViewModel<Uintra20.Core.HomePage.HomePageViewModel>(homePageModel);
            var centralFeedpanel = (CentralFeedPanelViewModel)homePageViewModel.Panels.Value.FirstOrDefault(i => i.ContentTypeAlias.Equals("centralFeedPanel"));
            var itemsPerPage = centralFeedpanel.ItemsPerRequest;
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
                Feed = GetFeedItems(pagedItemsList, settings),
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



        private (IEnumerable<IFeedItem> activities, int totalCount) GetLatestActivities(Enum activityType,
            int activityAmount)
        {
            var items = GetCentralFeedItems(activityType).ToArray();
            var filteredItems = FilterLatestActivities(items).Take(activityAmount);
            var sortedItems = Sort(filteredItems, activityType);

            return (sortedItems, items.Length);
        }

        private FeedItemViewModel MapFeedItemToViewModel(IFeedItem i, Dictionary<int, FeedSettings> settings)
        {
            var options = GetActivityFeedOptions(i.Id);

            var activity = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(i.Type).GetPreviewModel(i.Id);

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
    }
}
