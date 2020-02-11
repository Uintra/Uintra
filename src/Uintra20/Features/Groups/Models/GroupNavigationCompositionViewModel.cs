using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Groups.Models
{
    public class GroupNavigationCompositionViewModel : ICompositionViewModel
    {
        public PropertyViewModel<string> NavigationTitle { get; set; }
        public PropertyViewModel<bool> ShowInMenu { get; set; }
        public PropertyViewModel<int> SortOrder { get; set; }
    }
}