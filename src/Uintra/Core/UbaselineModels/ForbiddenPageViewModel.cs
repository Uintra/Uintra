using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra.Core.UbaselineModels
{
    public class ForbiddenPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }

        public PropertyViewModel<INodeViewModel[]> RightColumnPanels { get; set; }
    }
}