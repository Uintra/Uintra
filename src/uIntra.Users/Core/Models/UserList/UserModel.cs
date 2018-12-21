﻿using System;
using Uintra.Core.User;
using Uintra.Users.Attributes;

namespace Uintra.Users.UserList
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [UIColumn(0, "Photo", "photo", ColumnType.Photo)]
        public string Photo { get; set; }

        [UIColumn(1, "Name", "fullName", SupportSorting = true)]
        public string DisplayedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [UIColumn(2, "Email", "email", ColumnType.Email, SupportSorting = true)]
        public string Email { get; set; }

        [UIColumn(3, "Phone", "phone", SupportSorting = false)]
        public string Phone { get; set; }

        [UIColumn(4, "Department", "department", SupportSorting = true)]
        public string Department { get; set; }

        public IIntranetUser User { get; set; }
        public string ProfileUrl { get; set; }
    }
}
