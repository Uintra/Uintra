using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Core.LinksPicker;
using Uintra20.Core.UbaselineModels;

namespace Uintra20.Features.Navigation.Models
{
    public class SharedLinkItemViewModel : NodeViewModel
    {
        public PropertyViewModel<string> LinksGroupTitle { get; set; }
        public PropertyViewModel<int> Sort { get; set; }
        public PropertyViewModel<UintraLinksPickerViewModel[]> Links { get; set; }

    }
}