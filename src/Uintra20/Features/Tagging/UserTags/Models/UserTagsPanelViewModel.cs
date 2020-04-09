using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Tagging.UserTags.Models
{
    public class UserTagsPanelViewModel : NodeViewModel
    {
        public string Title { get; set; }
        public PropertyViewModel<IEnumerable<string>> Tags { get; set; }
    }
}