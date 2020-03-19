using System;

namespace Uintra20.Features.UserList.Models
{
    public class ActiveMemberSearchQuery
    {
        public string Text { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderingString { get; set; }
        public Guid? GroupId { get; set; }
        public bool MembersOfGroup { get; set; }
    }
}