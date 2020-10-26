using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra.Features.Tagging.UserTags.Models
{
    public class UserTagItemModel : NodeModel
    {
        public PropertyModel<string> Text { get; set; }
    }
}