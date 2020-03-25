using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Core.HomePage
{
    public class HomePageModel : NodeModel, IPanelSettingsComposition, IUintraNavigationComposition
    {
        public PanelSettingsCompositionModel PanelSettings { get; set; }
        public PropertyModel<PanelContainerModel> Panels {get;set;}
        public NavigationCompositionModel Navigation { get; set; }
        public PropertyModel<bool> ShowInSubMenu { get; set; }
        public PropertyModel<int> UserListPage { get; set; }
    }
}