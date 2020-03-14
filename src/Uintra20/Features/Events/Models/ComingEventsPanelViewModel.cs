using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Events.Models
{
    public class ComingEventsPanelViewModel : NodeViewModel
    {
        public PropertyViewModel<string> Title { get; set; }
        public IEnumerable<ComingEventViewModel> Events { get; set; }
    }
}