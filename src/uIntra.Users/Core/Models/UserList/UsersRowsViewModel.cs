using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Users
{
    public class UsersRowsViewModel
    {
        public IEnumerable<ProfileColumnModel> SelectedColumns { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
    }
}
