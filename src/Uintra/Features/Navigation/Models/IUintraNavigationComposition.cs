using UBaseline.Shared.Navigation;
using UBaseline.Shared.Property;

namespace Uintra.Features.Navigation.Models
{
    public interface IUintraNavigationComposition : INavigationComposition
    {
        PropertyModel<bool> ShowInMenu { get; set; }
    }
}