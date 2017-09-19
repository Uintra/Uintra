using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : FeedControllerBase
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICentralFeedLinkService _centralFeedLinkService;

        protected override string OverviewViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedDetailsView.cshtml";
        protected override string CreateViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedCreateView.cshtml";
        protected override string EditViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedEditView.cshtml";
        protected override string ListViewPath => "~/App_Plugins/CentralFeed/View/CentralFeedList.cshtml";
        protected override string NavigationViewPath => "~/App_Plugins/CentralFeed/View/Navigation.cshtml";
        protected override string LatestActivitiesViewPath => "~/App_Plugins/LatestActivities/View/LatestActivities.cshtml";

        protected CentralFeedControllerBase(
            ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IFeedTypeProvider centralFeedTypeProvider,
            ICentralFeedLinkService centralFeedLinkService)
            : base(centralFeedContentHelper, subscribeService, centralFeedService, intranetUserService)
        {
            _centralFeedService = centralFeedService;
            _centralFeedContentHelper = centralFeedContentHelper;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _centralFeedLinkService = centralFeedLinkService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        #region Actions
        [HttpGet]
        public virtual ActionResult Overview()
        {
            var model = GetOverviewModel();
            return PartialView(OverviewViewPath, model);
        }

        [HttpGet]
        public ActionResult Create(int typeId)
        {
            var activityType = _centralFeedTypeProvider.Get(typeId);
            var viewModel = GetCreateViewModel(activityType);
            return PartialView(CreateViewPath, viewModel);
        }

        [HttpGet]
        public virtual ActionResult Details(Guid id)
        {
            var viewModel = GetDetailsViewModel(id);
            return PartialView(DetailsViewPath, viewModel);
        }


        [HttpGet]
        public virtual ActionResult Edit(Guid id)
        {
            var viewModel = GetEditViewModel(id);
            return PartialView(EditViewPath, viewModel);
        }

        public virtual ActionResult OpenFilters()
        {
            var feedState = _centralFeedContentHelper.GetFiltersState<FeedFiltersState>();
            feedState.IsFiltersOpened = !feedState.IsFiltersOpened;
            _centralFeedContentHelper.SaveFiltersState(feedState);
            return new EmptyResult();
        }

        public virtual ActionResult List(FeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider.Get(model.TypeId);
            var items = GetCentralFeedItems(centralFeedType).ToList();

            if (IsEmptyFilters(model.FilterState, _centralFeedContentHelper.CentralFeedCookieExists()))
            {
                model.FilterState = GetFilterStateModel();
            }

            var tabSettings = _centralFeedService.GetSettings(centralFeedType);

            var filteredItems = ApplyFilters(items,model.FilterState, tabSettings).ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(filteredItems);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var centralFeedModel = GetFeedListViewModel(model, filteredItems, centralFeedType);
            var filterState = MapToFilterState(centralFeedModel.FilterState);
            _centralFeedContentHelper.SaveFiltersState(filterState);

            return PartialView(ListViewPath, centralFeedModel);
        }

        public virtual ActionResult LatestActivities(LatestActivitiesPanelModel panelModel)
        {
            var viewModel = GetLatestActivities(panelModel);
            return PartialView(LatestActivitiesViewPath, viewModel);
        }


        #endregion

        protected virtual FeedListViewModel GetFeedListViewModel(FeedListModel model, List<IFeedItem> filteredItems, IIntranetType centralFeedType)
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
            var activitySettings = settings
                .ToDictionary(s => s.Type.Id);

            var result = items
                .Select(i => new FeedItemViewModel()
                {
                    Item = i,
                    Links = _centralFeedLinkService.GetLinks(i.Id),
                    ControllerName = activitySettings[i.Type.Id].Controller
                });

            return result;
        }

        protected virtual CentralFeedOverviewModel GetOverviewModel()
        {
            var tabType = _centralFeedContentHelper.GetTabType(CurrentPage);
            var centralFeedState = _centralFeedContentHelper.GetFiltersState<FeedFiltersState>();

            var activityTabs = _centralFeedContentHelper.GetTabs(CurrentPage).Map<List<FeedTabViewModel>>();

            var model = new CentralFeedOverviewModel
            {
                Tabs = activityTabs,
                TabsWithCreateUrl = GetTabsWithCreateUrl(activityTabs),
                CurrentType = tabType,
                IsFiltersOpened = centralFeedState.IsFiltersOpened
            };
            return model;
        }

        protected virtual LatestActivitiesViewModel GetLatestActivities(LatestActivitiesPanelModel panelModel)
        {
            var settings = _centralFeedService.GetAllSettings();
            var activitiesType = _centralFeedTypeProvider.Get(panelModel.ActivityTypeId);

            var latestActivities = GetCentralFeedItems(activitiesType).Take(panelModel.ActivityAmount);
            var feedItems = GetFeedItems(latestActivities, settings);
            var tab = GetTabForActivityType(activitiesType);

            return new LatestActivitiesViewModel()
            {
                Title = panelModel.Title,
                Teaser = panelModel.Teaser,
                Feed = feedItems,
                Tab = tab
            };
        }

        protected virtual IEnumerable<IFeedItem> GetCentralFeedItems(IIntranetType type)
        {
            if (IsTypeForAllActivities(type))
            {
                var items = _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate);
                return items;
            }

            return _centralFeedService.GetFeed(type);
        }


        private FeedTabViewModel GetTabForActivityType(IIntranetType activitiesType)
        {
            var result = _centralFeedContentHelper
                .GetTabs(CurrentPage)
                .Single(el => el.Type.Id == activitiesType.Id)
                .Map<FeedTabViewModel>();
            return result;
        }

        // TODO : duplication
        protected virtual CreateViewModel GetCreateViewModel(IIntranetType activityType)
        {
            var links = _centralFeedLinkService.GetCreateLinks(activityType);

            var settings = _centralFeedService.GetSettings(activityType);

            return new CreateViewModel()
            {
                Links = links,
                Settings = settings
            };
        }

        protected virtual EditViewModel GetEditViewModel(Guid id)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = _centralFeedLinkService.GetLinks(id);

            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);

            var viewModel = new EditViewModel()
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return viewModel;
        }

        protected virtual DetailsViewModel GetDetailsViewModel(Guid id)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = _centralFeedLinkService.GetLinks(id);

            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);

            var viewModel = new DetailsViewModel()
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return viewModel;
        }
    }
}