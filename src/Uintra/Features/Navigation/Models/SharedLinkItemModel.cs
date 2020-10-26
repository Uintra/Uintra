using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra.Core.LinksPicker;
using Uintra.Core.UbaselineModels;

namespace Uintra.Features.Navigation.Models
{
    public class SharedLinkItemModel : NodeModel
    {
        public PropertyModel<string> LinksGroupTitle { get; set; }
        public PropertyModel<UintraLinksPickerModel[]> Links { get; set; }
        public PropertyModel<int> Sort { get; set; }
    }
}