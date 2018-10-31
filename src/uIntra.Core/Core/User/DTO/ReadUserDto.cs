namespace Uintra.Core.User.DTO
{
    public class ReadUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IntranetRolesEnum Role { get; set; }
    }
}
