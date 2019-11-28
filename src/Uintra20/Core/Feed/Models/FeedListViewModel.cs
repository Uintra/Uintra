using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed.Settings;

namespace Uintra20.Core.Feed.Models
{
    public class FeedListViewModel
    {
        public Enum Type { get; set; }
        public IEnumerable<FeedItemViewModel> Feed { get; set; } = Enumerable.Empty<FeedItemViewModel>();
        public FeedTabSettings TabSettings { get; set; }
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }
        public FeedFilterStateViewModel FilterState { get; set; }
        public bool IsReadOnly { get; set; }
    }
}