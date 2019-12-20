using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.UintraPanels.LastActivities.Helpers;

namespace Uintra20.Features.CentralFeed.Web
{
    public class CentralFeedApiController : UBaselineApiController
    {
        private readonly ILatestActivitiesHelper _latestActivitiesHelper;

        public CentralFeedApiController(ILatestActivitiesHelper latestActivitiesHelper)
        {
            _latestActivitiesHelper = latestActivitiesHelper;
        }

        [HttpGet]
        public string AvailableActivityTypes()
        {
            return _latestActivitiesHelper.AvailableActivityTypes();
        }

        [HttpPost]
        public FeedListViewModel FeedList(FeedListModel model)
        {
            return _latestActivitiesHelper.GetFeedListViewModel(model);
        }
    }
}