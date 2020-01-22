using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.Member.Models;

namespace Uintra20.Core.Member.Profile.Models
{
    public class ProfilePageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public ProfileViewModel Profile { get; set; }
    }
}