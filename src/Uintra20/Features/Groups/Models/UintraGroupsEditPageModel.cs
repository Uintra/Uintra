using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Groups.Models
{
    public class UintraGroupsEditPageModel : NodeModel, IPanelsComposition, IGroupNavigationComposition, IPageSettingsComposition
    {
        public GroupNavigationCompositionModel GroupNavigation { get; set; }
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public PageSettingsCompositionModel PageSettings { get; set; }
    }
}