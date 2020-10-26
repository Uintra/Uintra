using System.ComponentModel.DataAnnotations;

namespace Uintra20.Core.Authentication.Models
{
    public class LoginModelBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login field is required")]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string ClientTimezoneId { get; set; }
    }
}