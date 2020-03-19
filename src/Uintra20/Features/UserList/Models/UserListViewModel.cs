using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra20.Features.UserList.Models
{
    public class UserListViewModel
    {
        public int DisplayedAmount { get; set; }
        public int AmountPerRequest { get; set; }
        public string Title { get; set; }
        public MembersRowsViewModel MembersRows { get; set; }
        public ProfileColumnModel OrderByColumn { get; set; }
        public bool IsLastRequest { get; set; }
    }
}