using Uintra.Core.User;

namespace Uintra.Users
{
    public class ProfileViewModel
    {
        public virtual string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public UserViewModel EditingUser { get; set; }
    }
}
