using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class GroupNavigationCompositionViewModel : ICompositionViewModel
    {
        public PropertyViewModel<string> NavigationTitle { get; set; }
        public PropertyViewModel<bool> ShowInMenu { get; set; }
    }
}