using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;
using Umbraco.Web.Mvc;

namespace uIntra.CentralFeed.Web
{
    public abstract class FeedControllerBase : SurfaceController
    {
        protected abstract string OverviewViewPath { get; }
        protected abstract string ListViewPath { get; }
        protected abstract string NavigationViewPath { get; }
        protected abstract string LatestActivitiesViewPath { get; }

        protected abstract string DetailsViewPath { get; }
        protected abstract string CreateViewPath { get; }
        protected abstract string EditViewPath { get; }

        protected virtual int ItemsPerPage => 8;

        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly ISubscribeService _subscribeService;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected FeedControllerBase(
            ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            ICentralFeedService centralFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            ICentralFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _centralFeedContentHelper = centralFeedContentHelper;
            _subscribeService = subscribeService;
            _centralFeedService = centralFeedService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _intranetUserService = intranetUserService;
        }

        public virtual ActionResult Overview()
        {
            var model = GetOverviewModel();
            return PartialView(OverviewViewPath, model);
        }

        [HttpGet]
        public virtual ActionResult Details(Guid id)
        {
            var viewModel = GetDetailsViewModel(id);
            return PartialView(DetailsViewPath, viewModel);
        }

        [HttpGet]
        public virtual ActionResult Create(int typeId)
        {
            var activityType = _centralFeedTypeProvider.Get(typeId);
            var viewModel = GetCreateViewModel(activityType);
            return PartialView(CreateViewPath, viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(Guid id)
        {
            var viewModel = GetEditViewModel(id);
            return PartialView(CreateViewPath, viewModel);
        }

        protected abstract CreateViewModel GetCreateViewModel(IIntranetType activityType);
        protected abstract EditViewModel GetEditViewModel(Guid id);
        protected abstract DetailsViewModel GetDetailsViewModel(Guid id);

        public virtual ActionResult List(CentralFeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider.Get(model.TypeId);
            var items = GetCentralFeedItems(centralFeedType).ToList();

            if (IsEmptyFilters(model.FilterState, _centralFeedContentHelper.CentralFeedCookieExists()))
            {
                model.FilterState = GetFilterStateModel();
            }

            var tabSettings = _centralFeedService.GetSettings(centralFeedType);

            var filteredItems = ApplyFilters(items, model.FilterState, tabSettings).ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(filteredItems);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var centralFeedModel = GetCentralFeedListViewModel(model, filteredItems, centralFeedType);
            var filterState = MapToFilterState(centralFeedModel.FilterState);
            _centralFeedContentHelper.SaveFiltersState(filterState);

            return PartialView(ListViewPath, centralFeedModel);
        }

        public virtual ActionResult Tabs()
        {
            return PartialView(NavigationViewPath, GetTypes().ToList());
        }

        public virtual JsonResult CacheVersion()
        {
            var version = _centralFeedService.GetFeedVersion(Enumerable.Empty<IFeedItem>());
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult AvailableActivityTypes()
        {
            var activityTypes = _centralFeedService
                .GetAllSettings()
                .Select(s => s.Type)
                .Select(a => new { a.Id, a.Name })
                .OrderBy(el => el.Id);

            return Json(activityTypes, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult LatestActivities(LatestActivitiesPanelModel panelModel)
        {
            var viewModel = GetLatestActivities(panelModel);
            return PartialView(LatestActivitiesViewPath, viewModel);
        }

        public virtual ActionResult OpenFilters()
        {
            var feedState = _centralFeedContentHelper.GetFiltersState<FeedFiltersState>();
            feedState.IsFiltersOpened = !feedState.IsFiltersOpened;
            _centralFeedContentHelper.SaveFiltersState(feedState);
            return new EmptyResult();
        }

        protected virtual FeedListViewModel GetCentralFeedListViewModel(FeedListModel model, List<IFeedItem> filteredItems, IIntranetType centralFeedType)
        {
            var take = model.Page * ItemsPerPage;
            var pagedItemsList = Sort(filteredItems, centralFeedType).Take(take).ToList();

            var settings = _centralFeedService.GetAllSettings();
            var tabSettings = settings
                .Single(s => s.Type.Id == model.TypeId)
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

        protected virtual IEnumerable<FeedItemViewModel> GetFeedItems(IEnumerable<IFeedItem> items, IEnumerable<FeedSettings> settings)
        {
            var activityTypes = GetInvolvedTypes(items);

            var activityServices = activityTypes
                .Select(t => _activitiesServiceFactory.GetService<IIntranetActivityService>(t.Id))
                .ToDictionary(s => s.ActivityType.Id);

            var activitySettings = settings
                .ToDictionary(s => s.Type.Id);

            var result = items
                .Select(i => new FeedItemViewModel()
                {
                    Item = i,
                    Links = activityServices[i.Type.Id].GetCentralFeedLinks(i.Id),
                    ControllerName = activitySettings[i.Type.Id].Controller
                });

            return result;
        }

        private static IEnumerable<IIntranetType> GetInvolvedTypes(IEnumerable<IFeedItem> items)
        {
            return items
                    .Select(i => i.Type)
                    .Distinct(new IntranetTypeComparer());
        }

        protected virtual IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings)
        {
            if (filterState.ShowSubscribed.GetValueOrDefault() && settings.HasSubscribersFilter)
            {
                items = items.Where(i => i is ISubscribable && _subscribeService.IsSubscribed(_intranetUserService.GetCurrentUser().Id, (ISubscribable)i));
            }

            if (filterState.ShowPinned.GetValueOrDefault() && settings.HasPinnedFilter)
            {
                items = items.Where(i => i.IsPinned);
            }

            return items;
        }

        protected virtual IList<IFeedItem> Sort(IEnumerable<IFeedItem> items, IIntranetType type)
        {
            var sortedItems = items.OrderByDescending(el => el.IsPinActual);
            switch (type.Id)
            {
                case (int)CentralFeedTypeEnum.Events:
                    sortedItems = sortedItems.ThenBy(i => i, new CentralFeedEventComparer());
                    break;
                case (int)CentralFeedTypeEnum.All:
                    sortedItems = sortedItems.ThenBy(i => i, new CentralFeedItemComparer());
                    break;
                default:
                    sortedItems = sortedItems.ThenByDescending(el => el.PublishDate);
                    break;
            }
            return sortedItems.ToList();
        }

        [Pure]
        protected virtual bool IsEmptyFilters(FeedFilterStateModel filterState, bool isCookiesExist)
        {
            return !isCookiesExist || filterState == null || !IsAnyFilterSet(filterState);
        }

        private bool IsAnyFilterSet(FeedFilterStateModel filterState)
        {
            return filterState.ShowPinned.HasValue
                || filterState.IncludeBulletin.HasValue
                || filterState.ShowSubscribed.HasValue;
        }

        protected virtual CentralFeedOverviewModel GetOverviewModel()
        {
            var tabType = _centralFeedContentHelper.GetTabType(CurrentPage);
            var centralFeedState = _centralFeedContentHelper.GetFiltersState<FeedFiltersState>();

            var model = new CentralFeedOverviewModel
            {
                Tabs = _centralFeedContentHelper.GetTabs(CurrentPage).Map<IEnumerable<CentralFeedTabViewModel>>(),
                CurrentType = tabType,
                IsFiltersOpened = centralFeedState.IsFiltersOpened
            };
            return model;
        }

        protected virtual FeedFilterStateModel GetFilterStateModel()
        {
            var stateModel = _centralFeedContentHelper.GetFiltersState<FeedFiltersState>();

            var result = new FeedFilterStateModel()
            {
                ShowPinned = stateModel.PinnedFilterSelected,
                IncludeBulletin = stateModel.BulletinFilterSelected,
                ShowSubscribed = stateModel.SubscriberFilterSelected
            };

            return result;
        }

        protected virtual IEnumerable<CentralFeedTypeModel> GetTypes()
        {
            var allSettings = _centralFeedService.GetAllSettings();
            foreach (var singleSetting in allSettings)
            {
                yield return new CentralFeedTypeModel
                {
                    Type = singleSetting.Type,
                    CreateUrl = singleSetting.CreatePage.Url,
                    TabUrl = singleSetting.OverviewPage.Url,
                    HasSubscribersFilter = singleSetting.HasSubscribersFilter
                };
            }
        }

        protected virtual LatestActivitiesViewModel GetLatestActivities(LatestActivitiesPanelModel panelModel)
        {
            var settings = _centralFeedService.GetAllSettings();
            var activitiesType = _centralFeedTypeProvider.Get(panelModel.ActivityTypeId);

            var latestActivities = GetCentralFeedItems(activitiesType).Take(panelModel.ActivityAmount);
            var feedItems = GetFeedItems(latestActivities, settings);
            var tab = GetTabForActivities(activitiesType);

            return new LatestActivitiesViewModel()
            {
                Title = panelModel.Title,
                Teaser = panelModel.Teaser,
                Feed = feedItems,
                Tab = tab
            };
        }

        private CentralFeedTabViewModel GetTabForActivities(IIntranetType activitiesType)
        {
            var result = _centralFeedContentHelper.GetTabs(CurrentPage).First(el => el.Type.Id == activitiesType.Id).Map<CentralFeedTabViewModel>();
            return result;
        }

        protected virtual IEnumerable<IFeedItem> GetCentralFeedItems(IIntranetType type)
        {
            if (type.Id == CentralFeedTypeEnum.All.ToInt())
            {
                var items = _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate);
                return items;
            }

            return _centralFeedService.GetFeed(type);
        }

        protected virtual FeedFilterStateViewModel MapToFilterStateViewModel(FeedFilterStateModel model)
        {
            return new FeedFilterStateViewModel()
            {
                ShowPinned = model.ShowPinned ?? false,
                IncludeBulletin = model.IncludeBulletin ?? false,
                ShowSubscribed = model.ShowSubscribed ?? false
            };
        }

        protected virtual FeedFiltersState MapToFilterState(FeedFilterStateViewModel model)
        {
            return new FeedFiltersState
            {
                PinnedFilterSelected = model.ShowPinned,
                BulletinFilterSelected = model.IncludeBulletin,
                SubscriberFilterSelected = model.ShowSubscribed
            };
        }
    }
}
