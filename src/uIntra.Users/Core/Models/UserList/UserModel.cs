using System;
using Uintra.Core.User;

namespace Uintra.Users
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [UIColumn(0, "Photo", "someIndexFiledName_photo", ColumnType.Photo)]
        public string Photo { get; set; }

        [UIColumn(1, "Name", "someIndexFiledName_name")]
        public string DisplayedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [UIColumn(2, "Email", "someIndexFiledName_email", ColumnType.Email)]
        public string Email { get; set; }
        public IIntranetUser User { get; set; }
    }
}
