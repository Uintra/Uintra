using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Core.Member.Profile.Edit.Models
{
    public class ProfileEditPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public ProfileEditViewModel Profile { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public IEnumerable<UserTag> AvailableTags { get; set; } = Enumerable.Empty<UserTag>();
    }
}