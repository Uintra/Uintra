using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationModel
    {
        public IEnumerable<TopNavigationItem> Items { get; set; }
        //public IIntranetMember CurrentMember { get; set; }
        //public string CentralUserListUrl { get; set; }
    }
}