using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Navigation.Models
{
    public class SharedLinkItemViewModel : NodeViewModel
    {
        public PropertyViewModel<string> LinksGroupTitle { get; set; }
        public PropertyViewModel<int> Sort { get; set; }
        public PropertyModel<LinksPickerViewModel[]> Links { get; set; }
        //public IEnumerable<SystemLinkItemViewModel> Links { get; set; } = Enumerable.Empty<SystemLinkItemViewModel>();

    }
}