using System;

namespace Uintra.Features.Groups.Models
{
    public class MemberGroupInviteModel
    {
        public Guid MemberId { get; set; }
        public Guid GroupId { get; set; }
    }
}