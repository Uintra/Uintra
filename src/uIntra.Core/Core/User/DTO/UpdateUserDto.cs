using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.User.DTO
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }

        [StringLength(128, MinimumLength = 2, ErrorMessage = "Allowed length 2 - 128")]
        public string FirstName { get; set; }

        [StringLength(128, MinimumLength = 2, ErrorMessage = "Allowed length 2 - 128")]
        public string LastName { get; set; }

        public int? NewMedia { get; set; }

        public bool DeleteMedia { get; set; }
    }
}