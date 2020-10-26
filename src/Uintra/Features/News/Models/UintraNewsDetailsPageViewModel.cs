using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Models;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.News.Models
{
    public class UintraNewsDetailsPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public NewsViewModel Details { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public bool CanEdit { get; set; }
        public bool IsGroupMember { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}