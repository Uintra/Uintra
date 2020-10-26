using System.ComponentModel.DataAnnotations;

namespace Uintra20.Core.Member.Models.Dto
{
    public class CreateMemberDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "FirstName Allowed length 1 - 50")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "LastName Allowed length 1 - 50")]
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Department { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is absent or empty")]
        public string Email { get; set; }

        public int? MediaId { get; set; }
    }
}