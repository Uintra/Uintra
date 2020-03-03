using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Social.Models
{
    public class SocialCreateDataViewModel
    {
        public string Title { get; set; }
        public IEnumerable<string> Dates { get; set; }
        public MemberViewModel Creator { get; set; }
        public IActivityCreateLinks Links { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public bool CanCreate { get; set; }
        public IEnumerable<UserTag> Tags { get; set; }
        public bool PinAllowed { get; set; }
        public bool CanEditOwner { get; set; }
        public Guid? GroupId { get; set; }
        public IEnumerable<IntranetMember> Members { get; set; } = Enumerable.Empty<IntranetMember>();
    }
}