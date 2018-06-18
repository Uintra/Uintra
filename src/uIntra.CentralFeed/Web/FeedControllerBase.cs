using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using Uintra.Core;
using Uintra.Core.Context;
using Uintra.Core.Extensions;
using Uintra.Core.Feed;
using Uintra.Core.User;
using Uintra.Subscribe;

namespace Uintra.CentralFeed.Web
{
    public abstract class FeedControllerBase : ContextController
    {
        protected abstract string OverviewViewPath { get; }
        protected abstract string ListViewPath { get; }

        protected abstract string DetailsViewPath { get; }
        protected abstract string CreateViewPath { get; }
        protected abstract string EditViewPath { get; }

        protected virtual int ItemsPerPage => 8;

        private readonly IFeedService _feedService;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;

        protected FeedControllerBase(
            ISubscribeService subscribeService,
            IFeedService feedService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            IFeedTypeProvider centralFeedTypeProvider,
            IContextTypeProvider contextTypeProvider): base(contextTypeProvider)
        {
            _feedService = feedService;
            _feedFilterStateService = feedFilterStateService;
        }

        [HttpGet]
        public virtual ActionResult OpenFilters()
        {
            var feedState = _feedFilterStateService.GetFiltersState();
            feedState.IsFiltersOpened = !feedState.IsFiltersOpened;
            _feedFilterStateService.SaveFiltersState(feedState);
            return new EmptyResult();
        }
        
        public virtual JsonResult AvailableActivityTypes()
        {
            var activityTypes = _feedService
                .GetAllSettings()
                .Where(s => !s.ExcludeFromAvailableActivityTypes)
                .Select(s => ( Id:s.Type.ToInt(), Name: s.Type.ToString()))
                .Select(a => new { a.Id, a.Name })
                .OrderBy(el => el.Id);

            return Json(activityTypes, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult CacheVersion()
        {
            var version = _feedService.GetFeedVersion(Enumerable.Empty<IFeedItem>());
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }        


        protected virtual IEnumerable<FeedItemViewModel> GetFeedItems(IEnumerable<IFeedItem> items, IEnumerable<FeedSettings> settings)
        {
            var activitySettings = settings
                .ToDictionary(s => s.Type.ToInt());

            var result = items
                .Select(i => MapFeedItemToViewModel(i, activitySettings));

            return result;
        }

        protected virtual FeedItemViewModel MapFeedItemToViewModel(IFeedItem i, Dictionary<int, FeedSettings> settings)
        {
            ActivityFeedOptions options = GetActivityFeedOptions(i.Id);
            return new FeedItemViewModel()
            {
                Activity = i,
                Options = options,
                ControllerName = settings[i.Type.ToInt()].Controller
            };
        }

        protected abstract ActivityFeedOptions GetActivityFeedOptions(Guid activityId);

        protected virtual IEnumerable<Enum> GetInvolvedTypes(IEnumerable<IFeedItem> items)
        {
            return items
                .Select(i => i.Type)
                .Distinct();
        }
        
        protected virtual IList<IFeedItem> SortForFeed(IEnumerable<IFeedItem> items, Enum type)
        {
            var sortedItems = Sort(items, type);
            return SortByPin(sortedItems).ToList();
        }

        protected virtual IEnumerable<IFeedItem> Sort(IEnumerable<IFeedItem> sortedItems, Enum type)
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

        protected virtual IEnumerable<IFeedItem> SortByPin(IEnumerable<IFeedItem> items) =>
            items.OrderByDescending(el => el.IsPinActual);

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
            var stateModel = _feedFilterStateService.GetFiltersState();

            var result = new FeedFilterStateModel()
            {
                ShowPinned = stateModel.PinnedFilterSelected,
                IncludeBulletin = stateModel.BulletinFilterSelected,
                ShowSubscribed = stateModel.SubscriberFilterSelected
            };

            return result;
        }

        protected static IEnumerable<ActivityFeedTabViewModel> GetTabsWithCreateUrl(IEnumerable<ActivityFeedTabViewModel> tabs) =>
            tabs.Where(t => !IsTypeForAllActivities(t.Type) && t.Links.Create.HasValue());

        protected static bool IsTypeForAllActivities(Enum type) =>
            type is CentralFeedTypeEnum.All;

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
