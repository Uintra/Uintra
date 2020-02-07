using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class GroupNavigationCompositionModel : ICompositionModel
    {
        public PropertyModel<string> NavigationTitle { get; set; }
        public PropertyModel<bool> ShowInMenu { get; set; }
    }
}