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
        private readonly IFeedService _feedService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected FeedControllerBase(
            ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            IFeedService feedService,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _centralFeedContentHelper = centralFeedContentHelper;
            _subscribeService = subscribeService;
            _feedService = feedService;
            _intranetUserService = intranetUserService;
        }


        #region Actions
        public virtual JsonResult AvailableActivityTypes()
        {
            var activityTypes = _feedService
                .GetAllSettings()
                .Select(s => s.Type)
                .Select(a => new { a.Id, a.Name })
                .OrderBy(el => el.Id);

            return Json(activityTypes, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult CacheVersion()
        {
            var version = _feedService.GetFeedVersion(Enumerable.Empty<IFeedItem>());
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected static IEnumerable<IIntranetType> GetInvolvedTypes(IEnumerable<IFeedItem> items)
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

        protected static IEnumerable<FeedTabViewModel> GetTabsWithCreateUrl(IEnumerable<FeedTabViewModel> tabs) => 
            tabs.Where(t => !IsTypeForAllActivities(t.Type) && t.Links.Create.IsNotNullOrEmpty());

        protected static bool IsTypeForAllActivities(IIntranetType type) =>
            type.Id == CentralFeedTypeEnum.All.ToInt();

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
