using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra.Core.UbaselineModels
{
    public class PageNotFoundPageModel: NodeModel, IPanelsComposition
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
    }
}