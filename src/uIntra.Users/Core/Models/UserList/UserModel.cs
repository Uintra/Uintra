using System;
using Uintra.Core.User;

namespace Uintra.Users
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Photo { get; set; }
        public string DisplayedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IIntranetUser User { get; set; }
    }
}
