using uIntra.Core.User;

namespace uIntra.Users
{
    public class ProfileViewModel
    {
        public virtual string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IIntranetUser EditingUser { get; set; }
    }
}
