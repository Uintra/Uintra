using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Feed.Models;

namespace Uintra.Features.CentralFeed.Models
{
    public class LatestActivitiesModel
    {
        public bool IsShowMore { get; set; }
        public IEnumerable<LatestActivitiesItemViewModel> FeedItems { get; set; } = Enumerable.Empty<LatestActivitiesItemViewModel>();
    }
}