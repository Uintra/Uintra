using UBaseline.Core.Node;
using Uintra20.Features.UintraPanels.LastActivities.Helpers;
using Uintra20.Features.UintraPanels.LastActivities.Models;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter : INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly ILatestActivitiesHelper _latestActivitiesHelper;

        public LatestActivitiesPanelViewModelConverter(ILatestActivitiesHelper latestActivitiesHelper)
        {
            _latestActivitiesHelper = latestActivitiesHelper;
        }
        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {
            var (isShowMore, feedItems) = _latestActivitiesHelper.GetFeedItems(node);

            viewModel.Feed = feedItems;
            viewModel.ShowSeeAllButton = isShowMore;
        }
    }
}