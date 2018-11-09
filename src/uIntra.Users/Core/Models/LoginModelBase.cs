using System.ComponentModel.DataAnnotations;

namespace Uintra.Users
{
    public class LoginModelBase
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public int ClientTimezoneOffset { get; set; }
        public GoogleAuthenticationSettings GoogleSettings { get; set; }
    }
}
