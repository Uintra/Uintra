using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;
using UBaseline.Shared.RightColumnPanelContainer;

namespace Uintra20.Core.UbaselineModels
{
    public class ForbiddenPageModel : NodeModel, IPanelsComposition,IRightColumnPanelsComposition
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public PropertyModel<PanelContainerModel> RightColumnPanels { get; set; }
    }
}