using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Users
{
    public class UserListViewModel
    {
        public int DisplayedAmount { get; set; }
        public int AmountPerRequest { get; set; }
        public string Title { get; set; }
        public IEnumerable<ProfileColumnModel> SelectedColumns { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
    }
}
