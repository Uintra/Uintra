using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class ForbiddenPageModel : NodeModel, IPanelsComposition
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
    }
}