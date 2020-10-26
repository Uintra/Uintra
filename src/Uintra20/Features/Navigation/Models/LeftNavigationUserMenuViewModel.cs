using Uintra20.Core.Member.Models;

namespace Uintra20.Features.Navigation.Models
{
    public class LeftNavigationUserMenuViewModel
    {
        public MemberViewModel CurrentMember { get; set; }
        public string ProfileLink { get; set; }
    }
}