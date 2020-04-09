using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;

namespace Uintra20.Features.Tagging.UserTags.Models
{
    public class UserTagsPanelModel : NodeModel
    {
        public IEnumerable<UserTag> UserTags { get; set; } = Enumerable.Empty<UserTag>();
    }
}