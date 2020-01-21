using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationItemViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Enum Type { get; set; }
    }
}