using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.UbaselineModels.RestrictedNode;

namespace Uintra20.Features.Groups.Models
{
    public class UintraGroupsDocumentsPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public GroupNavigationCompositionViewModel GroupNavigation { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public Guid? GroupId { get; set; }
        public bool CanUpload { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}