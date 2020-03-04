using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Controllers;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.Settings;
using Uintra20.Core.Feed.State;
using Uintra20.Features.CentralFeed.Helpers;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed.Web
{
    public class CentralFeedApiController : UBaselineApiController
    {
        private readonly ICentralFeedHelper _centralFeedHelper;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IGroupFeedService _groupFeedService;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedPresentationService _feedPresentationService;
        private readonly IFeedFilterService _feedFilterService;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _requestContext;

        public CentralFeedApiController(
            ICentralFeedHelper centralFeedHelper,
            IFeedTypeProvider feedTypeProvider,
            IGroupFeedService groupFeedService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            ICentralFeedService centralFeedService,
            IFeedPresentationService feedPresentationService,
            IFeedFilterService feedFilterService,
            INodeModelService nodeModelService,
            IUBaselineRequestContext requestContext
            )
        {
            _centralFeedHelper = centralFeedHelper;
            _feedTypeProvider = feedTypeProvider;
            _groupFeedService = groupFeedService;
            _feedFilterStateService = feedFilterStateService;
            _centralFeedService = centralFeedService;
            _feedPresentationService = feedPresentationService;
            _feedFilterService = feedFilterService;
            _nodeModelService = nodeModelService;
            _requestContext = requestContext;
        }

        [System.Web.Http.HttpGet]
        public string AvailableActivityTypes()
        {
            return _centralFeedHelper.AvailableActivityTypes();
        }

        [System.Web.Http.HttpPost]
        public FeedListViewModel FeedList(FeedListModel model)
        {
            var centralFeedType = _feedTypeProvider[model.TypeId];

            if (IsEmptyFilters(model.FilterState))
            {
                model.FilterState = GetFilterStateModel();
            }

            var items = model.GroupId.HasValue ? _centralFeedHelper.GetGroupFeedItems(centralFeedType, model.GroupId.Value).ToList() : _centralFeedHelper.GetCentralFeedItems(centralFeedType).ToList();
            if (!items.Any()) return new FeedListViewModel();

            var tabSettings = _groupFeedService.GetSettings(centralFeedType);
            var filteredItems = _feedFilterService.ApplyFilters(items, model.FilterState, tabSettings).ToList();

            var centralFeedModel = CreateFeedList(model, filteredItems, centralFeedType);

            return centralFeedModel;
        }


        private FeedListViewModel CreateFeedList(FeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
        {
            var centralFeedPanel = GetCentralFeedPanel();
            var itemsPerPage = centralFeedPanel.ItemsPerRequest;
            var skip = itemsPerPage * (model.Page - 1);

            var feed = SortForFeed(filteredItems, centralFeedType)
                .Skip(skip)
                .Take(itemsPerPage)
                .Select(fi => _feedPresentationService.GetPreviewModel(fi, model.GroupId.HasValue))
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
                Feed = feed,
                TabSettings = tabSettings,
                Type = centralFeedType,
                BlockScrolling = filteredItems.Count() < itemsPerPage,
                FilterState = MapToFilterStateViewModel(model.FilterState)
            };
        }


        private IEnumerable<IFeedItem> SortForFeed(IEnumerable<IFeedItem> items, Enum type)
        {
            var sortedItems = _centralFeedHelper.Sort(items, type);

            return sortedItems.OrderByDescending(el => el.IsPinActual)
                .ToArray();
        }

        private CentralFeedPanelViewModel GetCentralFeedPanel()
        {
            var homePageModel = _nodeModelService.GetByAlias<Core.HomePage.HomePageModel>("homePage", _requestContext.HomeNode.RootId);
            var homePageViewModel = _nodeModelService.GetViewModel<Core.HomePage.HomePageViewModel>(homePageModel);
            var centralFeedPanel = (CentralFeedPanelViewModel)homePageViewModel.Panels.Value.FirstOrDefault(i => i.ContentTypeAlias.Equals("centralFeedPanel"));
            return centralFeedPanel;
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

    }
}