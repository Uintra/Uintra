using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Models;

namespace Uintra20.Features.CentralFeed.Models
{
    public class CountableLatestActivities
    {
        public IEnumerable<IFeedItem> Activities = Enumerable.Empty<IFeedItem>();
        public int TotalCount { get; set; }
    }
}