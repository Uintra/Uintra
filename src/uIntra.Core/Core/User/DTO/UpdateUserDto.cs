using System;

namespace Uintra.Core.User.DTO
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? NewMedia { get; set; }
        public bool DeleteMedia { get; set; }
    }
}