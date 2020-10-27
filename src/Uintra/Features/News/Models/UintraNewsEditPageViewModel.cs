using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.Member.Entities;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Models;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Links.Models;

namespace Uintra.Features.News.Models
{
    public class UintraNewsEditPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public NewsViewModel Details { get; set; }
        public bool CanEditOwner { get; set; }
        public bool PinAllowed { get; set; }
        public IEnumerable<IntranetMember> Members { get; set; } = Enumerable.Empty<IntranetMember>();
        public string AllowedMediaExtensions { get; set; }
        public IActivityLinks Links { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}