using System.Collections.Generic;

namespace Uintra.Users.UserList
{
    public class UsersRowsViewModel
    {
        public IEnumerable<ProfileColumnModel> SelectedColumns { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
    }
}
