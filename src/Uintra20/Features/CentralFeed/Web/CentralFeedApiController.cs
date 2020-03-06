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
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedPresentationService _feedPresentationService;
        private readonly IFeedFilterService _feedFilterService;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _requestContext;

        public CentralFeedApiController(
            ICentralFeedHelper centralFeedHelper,
            IFeedTypeProvider feedTypeProvider,
            IGroupFeedService groupFeedService,
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

            var items = model.GroupId.HasValue ? _centralFeedHelper.GetGroupFeedItems(centralFeedType, model.GroupId.Value).ToList() : _centralFeedHelper.GetCentralFeedItems(centralFeedType).ToList();
            if (!items.Any()) return new FeedListViewModel();

            var tabSettings = GetTabSettings(model.GroupId.HasValue, centralFeedType);

            var filteredItems = _feedFilterService.ApplyFilters(items, model.FilterState, tabSettings).ToList();

            var centralFeedModel = GetFeedListModel(model, filteredItems, centralFeedType);

            return centralFeedModel;
        }

        private FeedListViewModel GetFeedListModel(FeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
        {
            var centralFeedPanel = GetCentralFeedPanel();

            var feed = SortForFeed(filteredItems, centralFeedType)
                .Skip(centralFeedPanel.ItemsPerRequest * (model.Page - 1))
                .Take(centralFeedPanel.ItemsPerRequest)
                .Select(fi => _feedPresentationService.GetPreviewModel(fi, model.GroupId.HasValue))
                .ToList();


            return new FeedListViewModel
            {
                Feed = feed,
                TabSettings = GetTabSettings(model),
                Type = centralFeedType,
                BlockScrolling = filteredItems.Count < centralFeedPanel.ItemsPerRequest,
            };
        }

        private FeedSettings GetTabSettings(bool isGroupFeed, Enum centralFeedType)
        {
            return isGroupFeed ? _groupFeedService.GetSettings(centralFeedType) : _centralFeedService.GetSettings(centralFeedType);
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

        private FeedTabSettings GetTabSettings(FeedListModel model)
        {
            return _centralFeedService
                .GetAllSettings()
                .Single(s => s.Type.ToInt() == model.TypeId)
                .Map<FeedTabSettings>();
        }
    }
}