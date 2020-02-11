using UBaseline.Core.Node;
using Uintra20.Features.CentralFeed.Helpers;
using Uintra20.Features.UintraPanels.LastActivities.Models;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter :
        INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly ICentralFeedHelper _centralFeedHelper;

        public LatestActivitiesPanelViewModelConverter(ICentralFeedHelper centralFeedHelper)
        {
            _centralFeedHelper = centralFeedHelper;
        }

        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {
            var feed = _centralFeedHelper.GetFeedItems(node);

            viewModel.Feed = feed.FeedItems;
            viewModel.ShowSeeAllButton = feed.IsShowMore;
        }
    }
}