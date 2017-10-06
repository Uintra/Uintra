using System;

namespace uIntra.Groups
{
    public class GroupMember
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid MemberId { get; set; }
    }
}