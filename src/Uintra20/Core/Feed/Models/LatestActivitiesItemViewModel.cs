using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Feed.Models
{
    public class LatestActivitiesItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public MemberViewModel Owner { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IActivityLinks Links { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}