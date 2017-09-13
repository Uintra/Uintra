using System;
using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class EditViewModel
    {
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
        public FeedSettings Settings { get; set; }
    }
}