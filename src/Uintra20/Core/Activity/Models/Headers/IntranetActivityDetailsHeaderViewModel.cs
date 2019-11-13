using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Links;
using Uintra20.Core.Location;
using Uintra20.Core.User;

namespace Uintra20.Core.Activity
{
    public class IntranetActivityDetailsHeaderViewModel
    {
        public string Title { get; set; }
        public Enum Type { get; set; }
        public MemberViewModel Owner { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IActivityLinks Links { get; set; }
        public ActivityLocation Location { get; set; }
    }
}