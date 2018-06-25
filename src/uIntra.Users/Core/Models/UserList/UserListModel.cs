using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Users
{
    public class UserListModel
    {
        public int DisplayedAmount { get; set; }
        public int AmountPerRequest { get; set; }
        public string Title { get; set; }
        public dynamic SelectedProperties { get; set; }
        public dynamic OrderBy { get; set; }
    }
}
