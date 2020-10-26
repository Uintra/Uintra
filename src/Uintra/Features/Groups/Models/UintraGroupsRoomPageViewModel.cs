using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Links.Models;
using Uintra.Features.Social.Models;

namespace Uintra.Features.Groups.Models
{
    public class UintraGroupsRoomPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public GroupNavigationCompositionViewModel GroupNavigation { get; set; }
        public Guid? GroupId { get; set; }
        public GroupViewModel GroupInfo { get; set; }
        public SocialCreatePageViewModel SocialCreateModel { get; set; }
        public UintraLinkModel CreateNewsLink { get; set; }
        public UintraLinkModel CreateEventsLink { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}