using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Core.UbaselineModels
{
    public class HeadingPageModel: NodeModel, IUintraNavigationComposition
    {
        public NavigationCompositionModel Navigation { get; set; }
        public PropertyModel<bool> ShowInSubMenu { get; set; }
    }
}
