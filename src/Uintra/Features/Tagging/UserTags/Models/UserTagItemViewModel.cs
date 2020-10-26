using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra.Features.Tagging.UserTags.Models
{
    public class UserTagItemViewModel : NodeViewModel
    {
        public PropertyViewModel<string> Text { get; set; }
    }
}