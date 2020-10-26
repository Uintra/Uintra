using System;

namespace Uintra20.Features.Groups.Models
{
    public class GroupMemberSubscriptionModel
    {
        public Guid MemberId { get; set; }
        public bool IsAdmin { get; set; }
    }
}