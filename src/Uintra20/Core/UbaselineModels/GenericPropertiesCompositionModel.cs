using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class GenericPropertiesCompositionModel : ICompositionModel
    {
        public PropertyModel<bool> IsDeleted { get; set; }
        public PropertyModel<string> IntranetUserId { get; set; }
    }
}