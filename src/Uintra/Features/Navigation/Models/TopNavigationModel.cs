using System.Collections.Generic;
using Uintra.Core.Member.Entities;

namespace Uintra.Features.Navigation.Models
{
    public class TopNavigationModel
    {
        public IntranetMember CurrentMember { get; set; }
        public IEnumerable<TopNavigationItem> Items { get; set; }
        //public IIntranetMember CurrentMember { get; set; }
        //public string CentralUserListUrl { get; set; }
    }
}