using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Models;
using Uintra.Core.UbaselineModels.RestrictedNode;

namespace Uintra.Features.Social.Models
{
    public class SocialCreatePageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public SocialCreateDataViewModel Data { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}