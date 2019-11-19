namespace Uintra20.Features.User.RelatedUser
{
    public class IntranetUser : IIntranetUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsValid => IsApproved && !IsLockedOut;
    }
}