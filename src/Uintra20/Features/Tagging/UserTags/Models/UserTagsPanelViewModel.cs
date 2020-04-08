using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelSettings;

namespace Uintra20.Features.Tagging.UserTags.Models
{
    public class UserTagsPanelViewModel : NodeViewModel
    {
        public PanelSettingsCompositionModel PanelSettings { get; set; }
        public string Title { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
    }
}