using Uintra20.Core.Member.Abstractions;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationModel
    {
        public IIntranetMember CurrentMember { get; set; }
        public string CentralUserListUrl { get; set; }
    }
}