using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.User.DTO
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }

        [StringLength(256, MinimumLength = 1, ErrorMessage = "Allowed length 1 - 256")]
        public string FirstName { get; set; }

        [StringLength(256, MinimumLength = 1, ErrorMessage = "Allowed length 1 - 256")]
        public string LastName { get; set; }

        public int? NewMedia { get; set; }

        public bool DeleteMedia { get; set; }
    }
}