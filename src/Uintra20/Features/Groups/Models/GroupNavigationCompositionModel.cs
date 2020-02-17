using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Groups.Models
{
    public class GroupNavigationCompositionModel : ICompositionModel
    {
        public PropertyModel<string> NavigationTitle { get; set; }
        public PropertyModel<bool> ShowInMenu { get; set; }
        public PropertyModel<int> SortOrder { get; set; }
    }
}