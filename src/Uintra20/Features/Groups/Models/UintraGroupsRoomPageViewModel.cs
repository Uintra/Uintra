using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Social.Models;

namespace Uintra20.Features.Groups.Models
{
    public class UintraGroupsRoomPageViewModel : UintraRestrictedNodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public GroupNavigationCompositionViewModel GroupNavigation { get; set; }
        public Guid? GroupId { get; set; }
        public GroupViewModel GroupInfo { get; set; }
        public SocialCreatePageViewModel SocialCreateModel { get; set; }
        public UintraLinkModel CreateNewsLink { get; set; }
        public UintraLinkModel CreateEventsLink { get; set; }
    }
}