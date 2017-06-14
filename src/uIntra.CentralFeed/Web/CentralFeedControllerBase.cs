using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.CentralFeed.App_Plugins.CentralFeed.Models;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
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

        private readonly ICentralFeedService _centralFeedService;
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ISubscribeService _subscribeService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        protected const int ItemsPerPage = 8;

        protected CentralFeedControllerBase(
            ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IActivitiesServiceFactory activitiesServiceFactory,
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper)
        {
            _centralFeedService = centralFeedService;
            _centralFeedContentHelper = centralFeedContentHelper;
            _activitiesServiceFactory = activitiesServiceFactory;
            _subscribeService = subscribeService;
            _intranetUserService = intranetUserService;
            _intranetUserContentHelper = intranetUserContentHelper;
        }

        public virtual ActionResult Overview()
        {
            var tabType = _centralFeedContentHelper.GetTabType(CurrentPage);

            var model = new CentralFeedOverviewModel
            {
                Tabs = _centralFeedContentHelper.GetTabs(CurrentPage).Map<IEnumerable<CentralFeedTabViewModel>>(),
                CurrentType = tabType
            };
            return PartialView(OverviewViewPath, model);
        }

        public virtual ActionResult List(CentralFeedListModel model)
        {
            var items = GetCentralFeedItems(model.Type).ToList();

            if (!_centralFeedContentHelper.CentralFeedCookieExists() || IsEmptyFilters(model))
            {
                RestoreFiltersState(model, _centralFeedContentHelper.GetFiltersState<CentralFeedFiltersStateModel>());
            }

            var tabSettings = _centralFeedService.GetSettings(model.Type);

            var filteredItems = ApplyFilters(items, model, tabSettings).ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(filteredItems);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var take = model.Page * ItemsPerPage;
            var pagedItemsList = Sort(filteredItems, model.Type).Take(take).ToList();

            FillActivityDetailLinks(pagedItemsList);

            var centralFeedModel = new CentralFeedListViewModel
            {
                Version = _centralFeedService.GetFeedVersion(filteredItems),
                Items = pagedItemsList,
                Settings = _centralFeedService.GetAllSettings(),
                Type = model.Type,
                BlockScrolling = filteredItems.Count < take,
                ShowPinned = model.ShowPinned ?? false,
                IncludeBulletin = model.IncludeBulletin ?? false,
                ShowSubscribed = model.ShowSubscribed ?? false
            };

            _centralFeedContentHelper.SaveFiltersState(new CentralFeedFiltersStateModel()
            {
                PinnedFilterSelected = centralFeedModel.ShowPinned,
                BulletinFilterSelected = centralFeedModel.IncludeBulletin,
                SubscriberFilterSelected = centralFeedModel.ShowSubscribed
            });
            return PartialView(ListViewPath, centralFeedModel);
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
            var activityTypes = _centralFeedService.GetAllSettings().Select(s => s.Type);
            var activityTypeModelList = activityTypes.Select(a => new { Id = a.GetHashCode(), Name = a.ToString() }).ToList();
            activityTypeModelList.Insert(0, new { Id = CentralFeedTypeEnum.All.GetHashCode(), Name = CentralFeedTypeEnum.All.ToString() });

            return Json(activityTypeModelList, JsonRequestBehavior.AllowGet);
        }

        protected virtual IEnumerable<ICentralFeedItem> GetCentralFeedItems(CentralFeedTypeEnum type)
        {
            if (type == CentralFeedTypeEnum.All)
            {
                var items = _centralFeedService.GetFeed();
                return items;
            }

            var activityType = type.GetHashCode().ToEnum<IntranetActivityTypeEnum>().GetValueOrDefault();
            return _centralFeedService.GetFeed(activityType);
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
                    HasSubscribersFilter = singleSetting.HasSubscribersFilter,

                };
            }
        }

        protected void FillActivityDetailLinks(IEnumerable<ICentralFeedItem> items)
        {
            var currentPage = GetCurrentPage();

            foreach (var type in items.Select(i => i.Type).Distinct())
            {
                var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(type);
                ViewData.SetActivityDetailsPageUrl(type, service.GetDetailsPage(currentPage).Url);
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

        protected virtual void RestoreFiltersState(CentralFeedListModel model, CentralFeedFiltersStateModel filtersState)
        {
            model.ShowPinned = filtersState.PinnedFilterSelected;
            model.IncludeBulletin = filtersState.BulletinFilterSelected;
            model.ShowSubscribed = filtersState.SubscriberFilterSelected;
        }

        protected virtual bool IsEmptyFilters(CentralFeedListModel model)
        {
            return !model.ShowPinned.HasValue && !model.IncludeBulletin.HasValue && !model.ShowSubscribed.HasValue;
        }

        protected virtual IList<ICentralFeedItem> Sort(IEnumerable<ICentralFeedItem> items, CentralFeedTypeEnum type)
        {
            if (type == CentralFeedTypeEnum.Events)
            {
                var events = items.OrderByDescending(el => el.IsPinActual).ThenBy(i => i, new CentralFeedEventComparer()).ToList();
                return events;
            }

            var itemList = items.OrderByDescending(el => el.IsPinActual).ThenByDescending(el => el.PublishDate).ToList();
            return itemList;
        }
    }
}