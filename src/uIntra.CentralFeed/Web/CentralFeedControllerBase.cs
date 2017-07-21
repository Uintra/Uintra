using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.CentralFeed.App_Plugins.CentralFeed.Models;
using uIntra.CentralFeed.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace uIntra.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/CentralFeed/View/CentralFeedList.cshtml";
        protected virtual string NavigationViewPath { get; } = "~/App_Plugins/CentralFeed/View/Navigation.cshtml";
        protected virtual string LatestActivitiesViewPath { get; } = "~/App_Plugins/LatestActivities/View/LatestActivities.cshtml";

        private readonly ICentralFeedService _centralFeedService;
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ISubscribeService _subscribeService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;
        protected const int ItemsPerPage = 8;

        protected CentralFeedControllerBase(
            ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            ICentralFeedTypeProvider centralFeedTypeProvider)
        {
            _centralFeedService = centralFeedService;
            _centralFeedContentHelper = centralFeedContentHelper;
            _activitiesServiceFactory = activitiesServiceFactory;
            _subscribeService = subscribeService;
            _intranetUserService = intranetUserService;
            _intranetUserContentHelper = intranetUserContentHelper;
            _centralFeedTypeProvider = centralFeedTypeProvider;
        }

        public ActionResult OpenFilters()
        {
            var centralFeedState = _centralFeedContentHelper.GetFiltersState<CentralFeedFiltersStateModel>();
            centralFeedState.IsFiltersOpened = !centralFeedState.IsFiltersOpened;
            _centralFeedContentHelper.SaveFiltersState(centralFeedState);
            return new EmptyResult();
        }

        public virtual ActionResult Overview()
        {
            var model = GetOverviewModel();
            return PartialView(OverviewViewPath, model);
        }

        public virtual ActionResult List(CentralFeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider.Get(model.Type);
            var items = GetCentralFeedItems(centralFeedType).ToList();

            RestoreFiltersState(model);

            var tabSettings = _centralFeedService.GetSettings(centralFeedType);

            var filteredItems = ApplyFilters(items, model, tabSettings).ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(filteredItems);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var centralFeedModel = GetCentralFeedListViewModel(model, filteredItems, centralFeedType);
            var filterStateModel = GetFilterStateModel(centralFeedModel);
            _centralFeedContentHelper.SaveFiltersState(filterStateModel);

            return PartialView(ListViewPath, centralFeedModel);
        }

        protected virtual CentralFeedOverviewModel GetOverviewModel()
        {
            var tabType = _centralFeedContentHelper.GetTabType(CurrentPage);
            var centralFeedState = _centralFeedContentHelper.GetFiltersState<CentralFeedFiltersStateModel>();

            var model = new CentralFeedOverviewModel
            {
                Tabs = _centralFeedContentHelper.GetTabs(CurrentPage).Map<IEnumerable<CentralFeedTabViewModel>>(),
                CurrentType = tabType,
                IsFiltersOpened = centralFeedState.IsFiltersOpened
            };
            return model;
        }

        protected virtual CentralFeedFiltersStateModel GetFilterStateModel(CentralFeedListViewModel centralFeedModel)
        {
            return new CentralFeedFiltersStateModel
            {
                PinnedFilterSelected = centralFeedModel.ShowPinned,
                BulletinFilterSelected = centralFeedModel.IncludeBulletin,
                SubscriberFilterSelected = centralFeedModel.ShowSubscribed
            };
        }

        protected virtual CentralFeedListViewModel GetCentralFeedListViewModel(CentralFeedListModel model, List<ICentralFeedItem> filteredItems, IIntranetType centralFeedType)
        {
            var take = model.Page * ItemsPerPage;
            var pagedItemsList = Sort(filteredItems, centralFeedType).Take(take).ToList();
            FillActivityDetailLinks(pagedItemsList);

            return new CentralFeedListViewModel
            {
                Version = _centralFeedService.GetFeedVersion(filteredItems),
                Items = pagedItemsList,
                Settings = _centralFeedService.GetAllSettings(),
                Type = centralFeedType,
                BlockScrolling = filteredItems.Count < take,
                ShowPinned = model.ShowPinned ?? false,
                IncludeBulletin = model.IncludeBulletin ?? false,
                ShowSubscribed = model.ShowSubscribed ?? false
            };
        }

        protected virtual void RestoreFiltersState(CentralFeedListModel model)
        {
            if (_centralFeedContentHelper.CentralFeedCookieExists() && !IsEmptyFilters(model))
            {
                return;
            }

            var filtersState = _centralFeedContentHelper.GetFiltersState<CentralFeedFiltersStateModel>();
            model.ShowPinned = filtersState.PinnedFilterSelected;
            model.IncludeBulletin = filtersState.BulletinFilterSelected;
            model.ShowSubscribed = filtersState.SubscriberFilterSelected;
        }

        public virtual ActionResult Tabs()
        {
            return PartialView(NavigationViewPath, GetTypes().ToList());
        }

        public virtual JsonResult CacheVersion()
        {
            var version = _centralFeedService.GetFeedVersion(Enumerable.Empty<ICentralFeedItem>());
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


        protected virtual LatestActivitiesViewModel GetLatestActivities(LatestActivitiesPanelModel panelModel)
        {
            IIntranetType activitiesType = _centralFeedTypeProvider.Get(panelModel.TypeOfActivities);

            var latestActivities = GetCentralFeedItems(activitiesType).Take(panelModel.NumberOfActivities);
            var settings = _centralFeedService.GetAllSettings();

            return new LatestActivitiesViewModel()
            {
                Title = panelModel.Title,
                Teaser = panelModel.Teaser,
                Settings = settings,
                Items = latestActivities
            };
        }

        protected virtual IEnumerable<ICentralFeedItem> GetCentralFeedItems(IIntranetType type)
        {
            if (type.Id == CentralFeedTypeEnum.All.ToInt())
            {
                var items = _centralFeedService.GetFeed().ToList();
                items.Sort(new CentralFeedComparer());
                return items;
            }

            return _centralFeedService.GetFeed(type);
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

        protected virtual void FillActivityDetailLinks(IEnumerable<ICentralFeedItem> items)
        {
            var currentPage = GetCurrentPage();

            foreach (var type in items.Select(i => i.Type).Distinct())
            {
                var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(type.Id);
                ViewData.SetActivityDetailsPageUrl(type.Id, service.GetDetailsPage(currentPage).Url);
            }

            var profilePageUrl = _intranetUserContentHelper.GetProfilePage().Url;
            ViewData.SetProfilePageUrl(profilePageUrl);
        }

        protected virtual IPublishedContent GetCurrentPage()
        {
            if (_centralFeedContentHelper.IsCentralFeedPage(CurrentPage))
            {
                return _centralFeedContentHelper.GetOverviewPage();
            }

            return null;
        }

        protected virtual IEnumerable<ICentralFeedItem> ApplyFilters(IEnumerable<ICentralFeedItem> items, CentralFeedListModel model, CentralFeedSettings settings)
        {
            if (model.ShowSubscribed.GetValueOrDefault() && settings.HasSubscribersFilter)
            {
                items = items.Where(i => i is ISubscribable && _subscribeService.IsSubscribed(_intranetUserService.GetCurrentUser().Id, (ISubscribable)i));
            }

            if (model.ShowPinned.GetValueOrDefault() && settings.HasPinnedFilter)
            {
                items = items.Where(i => i.IsPinned);
            }

            return items;
        }

        protected virtual bool IsEmptyFilters(CentralFeedListModel model)
        {
            return !model.ShowPinned.HasValue && !model.IncludeBulletin.HasValue && !model.ShowSubscribed.HasValue;
        }

        protected virtual IList<ICentralFeedItem> Sort(IEnumerable<ICentralFeedItem> items, IIntranetType type)
        {
            if (type.Id == CentralFeedTypeEnum.Events.ToInt())
            {
                var events = items.OrderByDescending(el => el.IsPinActual).ThenBy(i => i, new CentralFeedEventComparer()).ToList();
                return events;
            }

            var itemList = items.OrderByDescending(el => el.IsPinActual).ThenByDescending(el => el.PublishDate).ToList();
            return itemList;
        }
    }
}