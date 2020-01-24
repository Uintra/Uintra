using System.Collections.Generic;
using Uintra20.Core.Member.Entities;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationModel
    {
        public IntranetMember CurrentMember { get; set; }
        public IEnumerable<TopNavigationItem> Items { get; set; }
        //public IIntranetMember CurrentMember { get; set; }
        //public string CentralUserListUrl { get; set; }
    }
}