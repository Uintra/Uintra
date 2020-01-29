using UBaseline.Shared.Panel;
using UBaseline.Shared.Property;
using UBaseline.Shared.Title;

namespace Uintra20.Features.UintraPanels.LastActivities.Models
{
    public class LatestActivitiesPanelModel :
        PanelModel,
        ITitleContainer
    {
        public PropertyModel<string> Title { get; set; }
        public PropertyModel<string> Teaser { get; set; }
        public PropertyModel<SelectedActivityModel> ActivityType { get; set; }
        public PropertyModel<int> CountToDisplay { get; set; }
    }
}