using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class FeedListViewModel
    {
        public IIntranetType Type { get; set; }
        public IEnumerable<FeedItemViewModel> Feed { get; set; } = Enumerable.Empty<FeedItemViewModel>();
        public FeedTabSettings TabSettings { get; set; }
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }
        public FeedFilterStateViewModel FilterState { get; set; }
        public bool IsReadOnly { get; set; }
    }
}