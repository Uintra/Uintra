using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Links;
using uIntra.Core.Location;
using uIntra.Core.User;

namespace uIntra.Core.Activity
{
    public class IntranetActivityDetailsHeaderViewModel
    {
        public string Title { get; set; }
        public Enum Type { get; set; }
        public IIntranetUser Owner { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IActivityLinks Links { get; set; }
        public ActivityLocation Location { get; set; }
    }
}