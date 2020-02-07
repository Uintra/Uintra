using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;
using Uintra20.Core.UbaselineModels;

namespace Uintra20.Features.Groups.Models
{
    public class UintraMyGroupsPageModel : NodeModel, IPanelsComposition, IGroupNavigationComposition, IPageSettingsComposition
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public GroupNavigationCompositionModel GroupNavigation { get; set; }
        public PageSettingsCompositionModel PageSettings { get; set; }
    }
}