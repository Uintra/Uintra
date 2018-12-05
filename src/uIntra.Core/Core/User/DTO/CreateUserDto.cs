using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.User.DTO
{
    public class CreateUserDto
    {
        [StringLength(256, MinimumLength = 1, ErrorMessage = "FirstName Allowed length 1 - 256")]
        public string FirstName { get; set; }

        [StringLength(256, MinimumLength = 1, ErrorMessage = "LastName Allowed length 1 - 256")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is absent or empty")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Role is absent or empty")]
        public IntranetRolesEnum Role { get; set; }

        public int? MediaId { get; set; }
    }
}
