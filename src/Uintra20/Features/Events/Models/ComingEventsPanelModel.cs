using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Events.Models
{
    public class ComingEventsPanelModel : NodeModel
    {
        public PropertyModel<string> Title { get; set; }
        public PropertyModel<int> EventsAmount { get; set; }
    }
}