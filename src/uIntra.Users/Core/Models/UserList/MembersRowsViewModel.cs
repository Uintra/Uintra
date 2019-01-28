using System;
using System.Collections.Generic;
using Uintra.Core.User;

namespace Uintra.Users.UserList
{
    public class MembersRowsViewModel
    {
        public IEnumerable<ProfileColumnModel> SelectedColumns { get; set; }
        public IEnumerable<MemberModel> Members { get; set; }
        public bool IsLastRequest { get; set; }
        public MemberViewModel CurrentMember { get; set; }
        public bool IsCurrentMemberAdmin { get; set; }
        public Guid? GroupId { get; set; }
    }
}
