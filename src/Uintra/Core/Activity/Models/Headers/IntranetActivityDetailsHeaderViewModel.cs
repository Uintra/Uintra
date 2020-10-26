using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Member.Models;
using Uintra.Features.Links.Models;
using Uintra.Features.Location.Models;

namespace Uintra.Core.Activity.Models.Headers
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