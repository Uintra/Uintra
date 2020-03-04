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
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.CentralFeed.Helpers
{
    public class CentralFeedHelper : ICentralFeedHelper
    {
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ICentralFeedContentProvider _contentProvider;
        private readonly IGroupFeedService _groupFeedService;
        private readonly IFeedFilterService _feedFilterService;
        private readonly IFeedPresentationService _feedPresentationService;

        public CentralFeedHelper(
            IActivitiesServiceFactory activitiesServiceFactory,
            ICentralFeedService centralFeedService,
            IFeedTypeProvider feedTypeProvider,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            INodeModelService nodeModelService,
            IUBaselineRequestContext requestContext,
            ICentralFeedContentProvider contentProvider,
            IGroupFeedService groupFeedService,
            IFeedFilterService feedFilterService,
            IFeedPresentationService feedPresentationService
            )
        {
            _feedTypeProvider = feedTypeProvider;
            _centralFeedService = centralFeedService;
            _feedFilterStateService = feedFilterStateService;
            activitiesServiceFactory.GetServices<IIntranetActivityService<IIntranetActivity>>();
            _nodeModelService = nodeModelService;
            _requestContext = requestContext;
            _contentProvider = contentProvider;
            _groupFeedService = groupFeedService;
            _feedFilterService = feedFilterService;
            _feedPresentationService = feedPresentationService;
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

        public FeedListViewModel GetFeedItems(FeedListModel model)
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



        public bool IsCentralFeedPage(IPublishedContent page)
        {
            return IsHomePage(page) || IsSubPage(page);
        }

        private bool IsHomePage(IPublishedContent page) =>
            _contentProvider.GetOverviewPage().Id == page.Id;

        private bool IsSubPage(IPublishedContent page) =>
            _contentProvider.GetRelatedPages().Any(c => c.IsAncestorOrSelf(page));

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


        private CentralFeedPanelViewModel GetCentralFeedPanel()
        {
            var homePageModel = _nodeModelService.GetByAlias<Core.HomePage.HomePageModel>("homePage", _requestContext.HomeNode.RootId);
            var homePageViewModel = _nodeModelService.GetViewModel<Core.HomePage.HomePageViewModel>(homePageModel);
            var centralFeedPanel = (CentralFeedPanelViewModel)homePageViewModel.Panels.Value.FirstOrDefault(i => i.ContentTypeAlias.Equals("centralFeedPanel"));
            return centralFeedPanel;
        }
    }
}
