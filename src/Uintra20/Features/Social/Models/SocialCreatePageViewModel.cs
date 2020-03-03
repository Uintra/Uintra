using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Groups;
using Uintra20.Features.Groups.Models;

namespace Uintra20.Features.Social.Models
{
    public class SocialCreatePageViewModel : NodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public SocialCreateDataViewModel Data { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}