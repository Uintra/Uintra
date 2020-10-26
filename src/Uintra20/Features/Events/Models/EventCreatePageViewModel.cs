using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups;
using Uintra20.Features.Groups.Models;

namespace Uintra20.Features.Events.Models
{
    public class EventCreatePageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public EventCreateDataViewModel Data { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}