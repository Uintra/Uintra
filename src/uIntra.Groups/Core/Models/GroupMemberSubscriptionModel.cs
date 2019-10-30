using System;

namespace Uintra.Groups
{
    public class GroupMemberSubscriptionModel
    {
        public Guid MemberId { get; set; }
        public bool IsAdmin { get; set; }
    }
}