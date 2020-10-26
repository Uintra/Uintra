using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.Member.Entities;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Models;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Events.Models
{
    public class EventEditPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public EventViewModel Details { get; set; }
        public bool CanEditOwner { get; set; }
        public bool PinAllowed { get; set; }
        public IEnumerable<IntranetMember> Members { get; set; } = Enumerable.Empty<IntranetMember>();
        public string AllowedMediaExtensions { get; set; }
        public IActivityLinks Links { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}