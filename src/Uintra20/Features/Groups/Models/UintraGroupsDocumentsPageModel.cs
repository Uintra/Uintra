using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Groups.Models
{
    public class UintraGroupsDocumentsPageModel : NodeModel, IPanelsComposition, IGroupNavigationComposition, IPageSettingsComposition
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public GroupNavigationCompositionModel GroupNavigation { get; set; }
        public PageSettingsCompositionModel PageSettings { get; set; }
    }
}