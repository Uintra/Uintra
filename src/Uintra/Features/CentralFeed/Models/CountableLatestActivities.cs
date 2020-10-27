using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Feed.Models;

namespace Uintra.Features.CentralFeed.Models
{
    public class CountableLatestActivities
    {
        public IEnumerable<IFeedItem> Activities = Enumerable.Empty<IFeedItem>();
        public int TotalCount { get; set; }
    }
}