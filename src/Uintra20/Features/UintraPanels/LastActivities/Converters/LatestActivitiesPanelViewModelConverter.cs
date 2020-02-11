using UBaseline.Core.Node;
using Uintra20.Features.CentralFeed.Helpers;
using Uintra20.Features.UintraPanels.LastActivities.Models;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter : INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly ICentralFeedHelper _centralFeedHelper;

        public LatestActivitiesPanelViewModelConverter(ICentralFeedHelper centralFeedHelper)
        {
            _centralFeedHelper = centralFeedHelper;
        }
        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {
            var (isShowMore, feedItems) = _centralFeedHelper.GetFeedItems(node);

            viewModel.Feed = feedItems;
            viewModel.ShowSeeAllButton = isShowMore;
        }
    }
}