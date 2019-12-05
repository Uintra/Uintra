using UBaseline.Core.Node;
using Uintra20.Features.UintraPanels.LastActivities.Helpers;
using Uintra20.Features.UintraPanels.LastActivities.Models;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter : INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly ILatestActivitiesPanelHelper _latestActivitiesPanelHelper;

        public LatestActivitiesPanelViewModelConverter(ILatestActivitiesPanelHelper latestActivitiesPanelHelper)
        {
            _latestActivitiesPanelHelper = latestActivitiesPanelHelper;
        }
        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {
            var (isShowMore, feedItems) = _latestActivitiesPanelHelper.GetFeedItems(node);

            viewModel.Feed = feedItems;
            viewModel.ShowSeeAllButton = isShowMore;
        }
    }
}