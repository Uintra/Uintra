using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.Location;
using Uintra.Core.User;

namespace Uintra.Core.Activity
{
    public class IntranetActivityDetailsHeaderViewModel
    {
        public string Title { get; set; }
        public Enum Type { get; set; }
        public UserViewModel Owner { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IActivityLinks Links { get; set; }
        public ActivityLocation Location { get; set; }
    }
}