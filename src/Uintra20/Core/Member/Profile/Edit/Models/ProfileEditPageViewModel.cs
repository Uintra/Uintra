using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.Member.Models;

namespace Uintra20.Core.Member.Profile.Edit.Models
{
    public class ProfileEditPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public ProfileEditModel Profile { get; set; }
    }
}