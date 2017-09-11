using System;
using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class DetailsViewModel
    {
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
        public CentralFeedSettings Settings { get; set; }
    }
}