using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using UBaseline.Shared.Title;

namespace Uintra20.Features.UintraPanels.LastActivities.Models
{
    public class LatestActivitiesPanelModel : 
        NodeModel,
        ITitleContainer
    {
        public PropertyModel<string> Title { get; set; }
        public PropertyModel<string> Teaser { get; set; }
        public PropertyModel<SelectedActivityModel> ActivityType { get; set; }
        public PropertyModel<int> CountToDisplay { get; set; }
    }
}