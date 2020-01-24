using UBaseline.Shared.Property;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Core.UbaselineModels
{
    public class HomePageModel : UBaseline.Shared.HomePage.HomePageModel, IUintraNavigationComposition
    {
        public PropertyModel<bool> ShowInSubMenu { get; set; }
    }
}