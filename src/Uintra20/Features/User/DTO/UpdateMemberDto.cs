using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra20.Features.User.DTO
{
    public class UpdateMemberDto
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "FirstName Allowed length 1 - 50")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is empty")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "LastName Allowed length 1 - 50")]
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Department { get; set; }

        public int? NewMedia { get; set; }

        public bool DeleteMedia { get; set; }
    }
}