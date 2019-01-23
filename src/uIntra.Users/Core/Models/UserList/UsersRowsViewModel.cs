using System;
using System.Collections.Generic;
using Uintra.Core.User;

namespace Uintra.Users.UserList
{
    public class UsersRowsViewModel
    {
        public IEnumerable<ProfileColumnModel> SelectedColumns { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
        public bool IsLastRequest { get; set; }
        public UserViewModel CurrentUser { get; set; }
        public bool IsCurrentUserAdmin { get; set; }
        public Guid? GroupId { get; set; }
    }
}
