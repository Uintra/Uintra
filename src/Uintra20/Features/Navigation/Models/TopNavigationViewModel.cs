using System.Collections.Generic;
using Uintra20.Core.Member.Models;


namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationViewModel
    {
        public MemberViewModel CurrentMember { get; set; }
        public IEnumerable<TopNavigationItemViewModel> Items { get; set; }
    }
}