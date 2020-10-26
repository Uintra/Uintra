using System;

namespace Uintra.Features.Groups.Models
{
    public class GroupMemberSubscriptionModel
    {
        public Guid MemberId { get; set; }
        public bool IsAdmin { get; set; }
    }
}