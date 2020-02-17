using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Localization;
using Uintra20.Features.CentralFeed.Enums;

namespace Uintra20.Features.CentralFeed.Builders
{
    public class ActivityTabsBuilder : IActivityTabsBuilder
    {
        private readonly IIntranetLocalizationService _localizationService;
        private readonly HashSet<ActivityFeedTabViewModel> _tabs = new HashSet<ActivityFeedTabViewModel>();
        private FeedFilterStateModel _feedFilterStateModel = new FeedFilterStateModel();

        public ActivityTabsBuilder(IIntranetLocalizationService localizationService)
        {
            _localizationService = localizationService;
        }


        public IEnumerable<ActivityFeedTabViewModel> Build()
        {
            return _tabs;
        }

        public ActivityTabsBuilder BuildSocialTab()
        {
            _tabs.Add(new ActivityFeedTabViewModel
            {
                IsActive = true,
                Type = CentralFeedTypeEnum.Social,
                Title = CentralFeedTypeEnum.Social.ToString()
            });

            return this;
        }

        public ActivityTabsBuilder BuildNewsTab()
        {
            _tabs.Add(new ActivityFeedTabViewModel
            {
                IsActive = false,
                Type = CentralFeedTypeEnum.News,
                Title = CentralFeedTypeEnum.News.ToString(),
                Filters = new[]
                {
                    new ActivityFeedTabFiltersViewModel(nameof(_feedFilterStateModel.ShowPinned), _localizationService.Translate("CentralFeedList.ShowPinned.chkbx"), false)
                }
            });

            return this;
        }

        public ActivityTabsBuilder BuildEventsTab()
        {
            _tabs.Add(new ActivityFeedTabViewModel
            {
                IsActive = false,
                Type = CentralFeedTypeEnum.Events,
                Title = CentralFeedTypeEnum.Events.ToString(),
                Filters = new[]
                {
                    new ActivityFeedTabFiltersViewModel(nameof(_feedFilterStateModel.ShowPinned), _localizationService.Translate("CentralFeedList.ShowPinned.chkbx"), false),
                    new ActivityFeedTabFiltersViewModel(nameof(_feedFilterStateModel.ShowSubscribed), _localizationService.Translate("CentralFeedList.ShowSubscribed.chkbx"), false)
                }
            });

            return this;
        }
    }
}