using System;
using System.Collections.Generic;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Social.Models
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