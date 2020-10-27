using System.Collections.Generic;
using Uintra.Core.Member.Models;


namespace Uintra.Features.Navigation.Models
{
    public class TopNavigationViewModel
    {
        public MemberViewModel CurrentMember { get; set; }
        public IEnumerable<TopNavigationItemViewModel> Items { get; set; }
    }
}