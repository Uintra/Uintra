using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.Mvc;

namespace uIntra.CentralFeed.Web
{
    public abstract class FeedControllerBase : SurfaceController
    {
        protected abstract string OverviewViewPath { get; }
        protected abstract string ListViewPath { get; }
        protected abstract string NavigationViewPath { get; }
        protected abstract string LatestActivitiesViewPath { get; }

        protected virtual int ItemsPerPage => 8;

        private readonly ICentralFeedContentHelper _centralFeedContentHelper;

        protected FeedControllerBase(ICentralFeedContentHelper centralFeedContentHelper)
        {
            _centralFeedContentHelper = centralFeedContentHelper;
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

        protected virtual FeedFilterStateViewModel MapToFilterStateModel(FeedFilterStateModel model)
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
