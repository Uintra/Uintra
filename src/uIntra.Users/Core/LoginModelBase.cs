namespace uIntra.Users.Core
{
    public class LoginModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public int ClientTimezoneOffset { get; set; }
    }
}
