using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra.Core.UbaselineModels
{
    public class GenericPropertiesCompositionViewModel : ICompositionViewModel
    {
        public PropertyViewModel<bool> IsDeleted { get; set; }
        public PropertyViewModel<string> IntranetUserId { get; set; }
    }
}