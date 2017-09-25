using System;
using uIntra.Core.Feed;

namespace uIntra.CentralFeed
{
    public class DetailsViewModel
    {
        public Guid Id { get; set; }
        public ActivityFeedOptions Options { get; set; }
        public FeedSettings Settings { get; set; }
    }
}