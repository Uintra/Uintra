using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.User.DTO
{
    public class CreateUserDto
    {
        [StringLength(128, MinimumLength = 2, ErrorMessage = "Allowed length 2 - 128")]
        public string FullName { get; set; }

        [StringLength(128, MinimumLength = 2, ErrorMessage = "Allowed length 2 - 128")]
        public string FirstName { get; set; }

        [StringLength(128, MinimumLength = 2, ErrorMessage = "Allowed length 2 - 128")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is absent or empty")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Role is absent or empty")]
        public IntranetRolesEnum Role { get; set; }

        public int? MediaId { get; set; }
    }
}
