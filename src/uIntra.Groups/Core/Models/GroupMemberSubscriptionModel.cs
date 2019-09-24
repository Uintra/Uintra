using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra.Groups
{
    public class GroupMemberSubscriptionModel
    {
        [Required(AllowEmptyStrings = false)]
        public Guid MemberId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public bool IsAdmin { get; set; }
    }
}