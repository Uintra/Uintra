using UBaseline.Core.Controllers;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.State;
using Uintra20.Features.CentralFeed.Helpers;

namespace Uintra20.Features.CentralFeed.Web
{
    public class CentralFeedApiController : UBaselineApiController
    {
        private readonly ICentralFeedHelper _centralFeedHelper;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;

        public CentralFeedApiController(ICentralFeedHelper centralFeedHelper, IFeedFilterStateService<FeedFiltersState> feedFilterStateService)
        {
            _centralFeedHelper = centralFeedHelper;
            _feedFilterStateService = feedFilterStateService;
        }

        [System.Web.Http.HttpGet]
        public string AvailableActivityTypes()
        {
            return _centralFeedHelper.AvailableActivityTypes();
        }

        [System.Web.Http.HttpPost]
        public FeedListViewModel FeedList(FeedListModel model)
        {
            return _centralFeedHelper.GetFeedListViewModel(model);
        }
    }
}