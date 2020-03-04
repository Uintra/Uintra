using UBaseline.Core.Controllers;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.CentralFeed.Helpers;

namespace Uintra20.Features.CentralFeed.Web
{
    public class CentralFeedApiController : UBaselineApiController
    {
        private readonly ICentralFeedHelper _centralFeedHelper;

        public CentralFeedApiController(ICentralFeedHelper centralFeedHelper)
        {
            _centralFeedHelper = centralFeedHelper;
        }

        [System.Web.Http.HttpGet]
        public string AvailableActivityTypes()
        {
            return _centralFeedHelper.AvailableActivityTypes();
        }

        [System.Web.Http.HttpPost]
        public FeedListViewModel FeedList(FeedListModel model)
        {
            return _centralFeedHelper.GetFeedItems(model);
        }
    }
}