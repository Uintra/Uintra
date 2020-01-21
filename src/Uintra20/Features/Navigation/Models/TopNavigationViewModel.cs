using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationViewModel
    {
        public IEnumerable<TopNavigationItemViewModel> Items { get; set; }
    }
}