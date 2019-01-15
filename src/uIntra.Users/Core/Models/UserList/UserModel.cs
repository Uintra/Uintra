using System;
using Uintra.Core.User;
using Uintra.Users.Attributes;

namespace Uintra.Users.UserList
{
    public class UserModel
    {
        [UIColumn(0, "Photo", "photo", ColumnType.Photo)]
        public string Photo { get; set; }

        [UIColumn(1, "Name", "fullName", ColumnType.Name, SupportSorting = true)]
        public string DisplayedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [UIColumn(2, "Email", "email", ColumnType.Email, SupportSorting = true)]
        public string Email { get; set; }

        [UIColumn(3, "Phone", "phone", ColumnType.Phone, SupportSorting = true)]
        public string Phone { get; set; }

        [UIColumn(4, "Department", "department", ColumnType.Department, SupportSorting = true)]
        public string Department { get; set; }

        public UserViewModel User { get; set; }

        public string ProfileUrl { get; set; }

        public bool IsGroupAdmin { get; set; }
    }
}
