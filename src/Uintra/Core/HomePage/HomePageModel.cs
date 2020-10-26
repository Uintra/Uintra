using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;
using Uintra.Features.Navigation.Models;

namespace Uintra.Core.HomePage
{
    public class HomePageModel : UBaseline.Shared.HomePage.HomePageModel
    {
        public PropertyModel<int?> UserListPage { get; set; }
    }
}