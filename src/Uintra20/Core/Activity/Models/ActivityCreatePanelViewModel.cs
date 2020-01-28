using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Core.Activity.Models
{
    public class ActivityCreatePanelViewModel : NodeViewModel
    {
        public PropertyViewModel<string> TabType { get; set; }
        public string Title { get; set; }
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public IEnumerable<string> Dates { get; set; }
        public MemberViewModel Creator { get; set; }
        public IActivityCreateLinks Links { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public int? MediaRootId { get; set; }
        public bool CanCreateBulletin { get; set; }
        public TagsPickerViewModel Tags { get; set; }
        public DateTime PublishDate { get; set; }
        public bool PinAllowed { get; set; }
        public bool CanEditOwner { get; set; }
        public IEnumerable<IntranetMember> Members { get; set; } = Enumerable.Empty<IntranetMember>();
    }
}