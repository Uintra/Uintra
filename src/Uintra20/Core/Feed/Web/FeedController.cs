using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Compent.Shared.Extensions;
using Compent.Shared.Extensions.Bcl;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Factories;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.Settings;
using Uintra20.Core.Feed.State;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Feed.Web
{
    public abstract class FeedController : ApiController
	{

        private readonly IFeedService _feedService;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        protected FeedController(
            IFeedService feedService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _feedService = feedService;
            _feedFilterStateService = feedFilterStateService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        [System.Web.Mvc.HttpGet]
        public virtual ActionResult OpenFilters()
        {
            var feedState = _feedFilterStateService.GetFiltersState();
            feedState.IsFiltersOpened = !feedState.IsFiltersOpened;
            _feedFilterStateService.SaveFiltersState(feedState);
            return new EmptyResult();
        }
        
        public string AvailableActivityTypes()
        {
            var activityTypes = _feedService
                .GetAllSettings()
                .Where(s => !s.ExcludeFromAvailableActivityTypes)
                .Select(s => ( Id:s.Type.ToInt(), Name: s.Type.ToString()))
                .Select(a => new { a.Id, a.Name })
                .OrderBy(el => el.Id);

            return activityTypes.ToJson();
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
            var options = GetActivityFeedOptions(i.Id);

            var activity = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(i.Type).GetViewModel(i.Id);

            return new FeedItemViewModel
            {
                Activity = activity,
                Options = options,
                ControllerName = settings[i.Type.ToInt()].Controller
            };
        }

        protected abstract ActivityFeedOptions GetActivityFeedOptions(Guid activityId);
        
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
                SubscriberFilterSelected = model.ShowSubscribed,
                IsFiltersOpened = _feedFilterStateService.GetFiltersState().IsFiltersOpened
            };
        }        
    }
}
