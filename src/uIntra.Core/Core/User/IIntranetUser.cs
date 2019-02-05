namespace Uintra.Core.User
{
    public interface IIntranetUser
    {
        int Id { get; set; }
        string Email { get; set; }
        string DisplayName { get; set; }
        bool IsSuperUser { get; set; }
        bool IsApproved { get; set; }
        bool IsLockedOut { get; set; }
    }
}