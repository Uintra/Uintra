using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.Events
{
    public class ComingEventViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IIntranetUser Owner { get; set; }
        public IActivityLinks Links { get; set; }
    }
}