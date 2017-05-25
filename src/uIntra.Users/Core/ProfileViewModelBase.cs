namespace uIntra.Users.Core
{
    public class ProfileViewModelBase
    {
        public virtual string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
