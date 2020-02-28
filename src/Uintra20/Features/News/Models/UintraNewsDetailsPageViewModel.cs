using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.News.Models
{
    public class UintraNewsDetailsPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public NewsViewModel Details { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public Guid? GroupId { get; set; }
        public bool RequiresGroupHeader { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool IsGroupMember { get; set; }
    }
}