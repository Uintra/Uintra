using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class ForbiddenPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }

        public PropertyViewModel<INodeViewModel[]> RightColumnPanels { get; set; }
    }
}