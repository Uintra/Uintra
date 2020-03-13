using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Events.Models
{
    public class ComingEventViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public MemberViewModel Owner { get; set; }
        public IActivityLinks Links { get; set; }
    }
}