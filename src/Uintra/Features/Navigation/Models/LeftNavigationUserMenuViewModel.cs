using Uintra.Core.Member.Models;

namespace Uintra.Features.Navigation.Models
{
    public class LeftNavigationUserMenuViewModel
    {
        public MemberViewModel CurrentMember { get; set; }
        public string ProfileLink { get; set; }
    }
}