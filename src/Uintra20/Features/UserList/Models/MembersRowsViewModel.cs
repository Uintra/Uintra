using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Core.Member.Models;

namespace Uintra20.Features.UserList.Models
{
    public class MembersRowsViewModel
    {
        public IEnumerable<ProfileColumnModel> SelectedColumns { get; set; }
        public IEnumerable<MemberModel> Members { get; set; }
        public bool IsLastRequest { get; set; }
        public MemberViewModel CurrentMember { get; set; }
        public bool IsCurrentMemberGroupAdmin { get; set; }
        public Guid? GroupId { get; set; }
        public bool IsInvite { get; set; }
    }
}