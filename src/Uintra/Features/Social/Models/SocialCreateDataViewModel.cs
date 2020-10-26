using System;
using System.Collections.Generic;
using Uintra.Core.Member.Models;
using Uintra.Features.Links.Models;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.Social.Models
{
    public class SocialCreateDataViewModel
    {
        public string Date { get; set; }
        public MemberViewModel Creator { get; set; }
        public IActivityCreateLinks Links { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public IEnumerable<UserTag> Tags { get; set; }
        public Guid? GroupId { get; set; }
    }
}