using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Feed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : FeedControllerBase
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly ICentralFeedContentService _centralFeedContentService;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICentralFeedLinkService _centralFeedLinkService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected override string OverviewViewPath => "~/App_Plugins/CentralFeed/View/Overview.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/CentralFeed/View/Details.cshtml";
        protected override string CreateViewPath => "~/App_Plugins/CentralFeed/View/Create.cshtml";
        protected override string EditViewPath => "~/App_Plugins/CentralFeed/View/Edit.cshtml";
        protected override string ListViewPath => "~/App_Plugins/CentralFeed/View/List.cshtml";
        protected virtual string NavigationViewPath => "~/App_Plugins/CentralFeed/View/Navigation.cshtml";
        protected virtual string LatestActivitiesViewPath => "~/App_Plugins/LatestActivities/View/LatestActivities.cshtml";

        protected CentralFeedControllerBase(
            ICentralFeedService centralFeedService,
            ICentralFeedContentService centralFeedContentService,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            ICentralFeedLinkService centralFeedLinkService)
            : base(centralFeedContentService, subscribeService, centralFeedService, intranetUserService)
        {
            _centralFeedService = centralFeedService;
            _centralFeedContentService = centralFeedContentService;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _centralFeedLinkService = centralFeedLinkService;
            _intranetUserService = intranetUserService;
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
        public ActionResult Create()
        {
            var activityType = _centralFeedContentService.GetCreateActivityType(CurrentPage);
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

        public virtual ActionResult List(FeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider.Get(model.TypeId);
            var items = GetCentralFeedItems(centralFeedType).ToList();

            if (IsEmptyFilters(model.FilterState, _centralFeedContentService.CentralFeedCookieExists()))
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

            var centralFeedModel = GetFeedListViewModel(model, filteredItems, centralFeedType);
            var filterState = MapToFilterState(centralFeedModel.FilterState);
            _centralFeedContentService.SaveFiltersState(filterState);

            return PartialView(ListViewPath, centralFeedModel);
        }

        [HttpGet]
        public virtual ActionResult OpenFilters()
        {
            var feedState = _centralFeedContentService.GetFiltersState<FeedFiltersState>();
            feedState.IsFiltersOpened = !feedState.IsFiltersOpened;
            _centralFeedContentService.SaveFiltersState(feedState);
            return new EmptyResult();
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
            var pagedItemsList = SortForFeed(filteredItems, centralFeedType).Take(take).ToList();

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

        protected override ActivityFeedOptions GetActivityFeedOptions(Guid activityId)
        {
            return new ActivityFeedOptions()
            {
                Links = _centralFeedLinkService.GetLinks(activityId)
            };
        }

        protected virtual CentralFeedOverviewModel GetOverviewModel()
        {
            var tabType = _centralFeedContentService.GetFeedTabType(CurrentPage);
            var centralFeedState = _centralFeedContentService.GetFiltersState<FeedFiltersState>();

            var activityTabs = GetActivityTabs().Map<List<ActivityFeedTabViewModel>>();

            var model = new CentralFeedOverviewModel
            {
                Tabs = activityTabs,
                TabsWithCreateUrl = IsUserAbleToCreateActivities() ? GetTabsWithCreateUrl(activityTabs) : Enumerable.Empty<ActivityFeedTabViewModel>(),
                CurrentType = tabType,
                IsFiltersOpened = centralFeedState.IsFiltersOpened
            };
            return model;
        }

        private bool IsUserAbleToCreateActivities() => _intranetUserService.GetCurrentUser().Role.Name != IntranetRolesEnum.UiUser.ToString();

        protected virtual IEnumerable<ActivityFeedTabModel> GetActivityTabs()
        {
            return _centralFeedContentService.GetTabs(CurrentPage);
        }

        protected virtual LatestActivitiesViewModel GetLatestActivities(LatestActivitiesPanelModel panelModel)
        {
            var settings = _centralFeedService.GetAllSettings();
            var activitiesType = _centralFeedTypeProvider.Get(panelModel.ActivityTypeId);

            var latestActivities = GetLatestActivities(activitiesType, panelModel.ActivityAmount);
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

        protected virtual IEnumerable<IFeedItem> GetLatestActivities(IIntranetType activityType, int activityAmount)
        {
            var items = GetCentralFeedItems(activityType).Take(activityAmount);
            return Sort(items, activityType);
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


        private ActivityFeedTabViewModel GetTabForActivityType(IIntranetType activitiesType)
        {
            var result = _centralFeedContentService
                .GetTabs(CurrentPage)
                .Single(el => el.Type.Id == activitiesType.Id)
                .Map<ActivityFeedTabViewModel>();
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
            var options = GetActivityFeedOptions(id);

            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);

            var viewModel = new DetailsViewModel()
            {
                Id = id,
                Options = options,
                Settings = settings
            };
            return viewModel;
        }
    }
}