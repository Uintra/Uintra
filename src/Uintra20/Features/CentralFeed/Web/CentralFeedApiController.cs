using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.UintraPanels.LastActivities.Helpers;

namespace Uintra20.Features.CentralFeed.Web
{
    public class CentralFeedApiController : UBaselineApiController
    {
        private readonly ICentralFeedHelper _centralFeedHelper;

        public CentralFeedApiController(ICentralFeedHelper centralFeedHelper)
        {
            _centralFeedHelper = centralFeedHelper;
        }

        [HttpGet]
        public string AvailableActivityTypes()
        {
            return _centralFeedHelper.AvailableActivityTypes();
        }

        [HttpPost]
        public FeedListViewModel FeedList(FeedListModel model)
        {
            return _centralFeedHelper.GetFeedListViewModel(model);
        }
    }
}