namespace Uintra.Core.User.DTO
{
    public class CreateUserDto
    {        
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }        
        public IntranetRolesEnum Role { get; set; }
        public int? MediaId { get; set; }
    }
}
