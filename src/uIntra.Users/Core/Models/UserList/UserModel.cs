using System;
using Uintra.Core.User;
using Uintra.Users.Attributes;

namespace Uintra.Users.UserList
{
    [UIColumn(0, "Name", "fullName", ColumnType.Name, SupportSorting = true)]
    [UIColumn(1, "Info", "info", ColumnType.Info)]
    //[UIColumn(2, "Button", "button", ColumnType.Button)]
    public class UserModel
    {
        public string Photo { get; set; }
        public string DisplayedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public UserViewModel User { get; set; }
        public string ProfileUrl { get; set; }
        public bool IsGroupAdmin { get; set; }
    }
}
