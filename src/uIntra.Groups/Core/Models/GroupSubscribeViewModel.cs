using System;

namespace uIntra.Groups
{
    public class GroupSubscribeViewModel
    {       
        public Guid GroupId { get; set; }

        public Guid? UserId { get; set; }

        public bool IsMember { get; set; }

        public int MembersCount { get; set; }
    }
}