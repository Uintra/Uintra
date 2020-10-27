using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra.Core.LinksPicker;
using Uintra.Core.UbaselineModels;

namespace Uintra.Features.Navigation.Models
{
    public class SharedLinkItemViewModel : NodeViewModel
    {
        public PropertyViewModel<string> LinksGroupTitle { get; set; }
        public PropertyViewModel<int> Sort { get; set; }
        public PropertyViewModel<UintraLinksPickerViewModel[]> Links { get; set; }

    }
}