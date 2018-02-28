using System;
using Uintra.Core.Feed;

namespace Uintra.CentralFeed
{
    public class DetailsViewModel
    {
        public Guid Id { get; set; }
        public ActivityFeedOptions Options { get; set; }
        public FeedSettings Settings { get; set; }
    }
}