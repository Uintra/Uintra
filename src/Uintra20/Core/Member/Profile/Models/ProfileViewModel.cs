using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Member.Profile.Models
{
    public class ProfileViewModel
    {
        public string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int? PhotoId { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public MemberViewModel EditingMember { get; set; }
    }
}