using Uintra20.Core.Member.Models;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationViewModel
    {
        public MemberViewModel CurrentMember { get; set; }
        public string CentralUserListUrl { get; set; }
    }
}