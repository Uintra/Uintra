using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Models;

namespace Uintra20.Features.CentralFeed.Models
{
    public class LoadableFeedItemModel
    {
        public bool IsShowMore { get; set; }
        public IEnumerable<FeedItemViewModel> FeedItems { get; set; } =
            Enumerable.Empty<FeedItemViewModel>();
    }
}