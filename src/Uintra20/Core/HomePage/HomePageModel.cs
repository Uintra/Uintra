using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Core.HomePage
{
    public class HomePageModel : UBaseline.Shared.HomePage.HomePageModel
    {
        public PropertyModel<int> UserListPage { get; set; }
    }
}