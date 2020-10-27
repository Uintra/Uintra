using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Models;

namespace Uintra.Features.Events.Models
{
    public class EventCreatePageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public EventCreateDataViewModel Data { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}