using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Navigation.Models
{
    public class HeadingPageModel: NodeModel, IUintraNavigationComposition
    {
        public NavigationCompositionModel Navigation { get; set; }
        public PropertyModel<bool> ShowInSubMenu { get; set; }
    }
}
