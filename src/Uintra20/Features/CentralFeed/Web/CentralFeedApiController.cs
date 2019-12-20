using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.UintraPanels.LastActivities.Helpers;

namespace Uintra20.Features.CentralFeed.Web
{
    public class CentralFeedApiController : UBaselineApiController
    {
        private readonly ILatestActivitiesPanelHelper _latestActivitiesPanelHelper;

        public CentralFeedApiController(ILatestActivitiesPanelHelper latestActivitiesPanelHelper)
        {
            _latestActivitiesPanelHelper = latestActivitiesPanelHelper;
        }

        [HttpGet]
        public string AvailableActivityTypes()
        {
            return _latestActivitiesPanelHelper.AvailableActivityTypes();
        }

        [HttpPost]
        public FeedListViewModel FeedList(FeedListModel model)
        {
            return _latestActivitiesPanelHelper.GetFeedListViewModel(model);
        }
    }
}