using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.UbaselineModels.RestrictedNode;

namespace Uintra.Features.Groups.Models
{
    public class UintraGroupsMembersPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public GroupNavigationCompositionViewModel GroupNavigation { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}