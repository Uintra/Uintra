using System;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Groups.Models
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        public bool HasImage { get; set; }
        public UintraLinkModel GroupUrl { get; set; }
        public string GroupImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MemberViewModel Creator { get; set; }
        public bool IsMember { get; set; }
        public int MembersCount { get; set; }
    }
}