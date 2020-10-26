using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra.Core.Member.Models;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Events.Models
{
    public class ComingEventViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public MemberViewModel Owner { get; set; }
        public IActivityLinks Links { get; set; }
        public string EventDate { get; set; }
        public string EventMonth { get; set; }
    }
}