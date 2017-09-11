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

    // TODO : how can I remove duplication?
    public class CreateViewModel
    {
        public ActivityLinks Links { get; set; }
        public CentralFeedSettings Settings { get; set; }
    }

    public class EditViewModel
    {
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
        public CentralFeedSettings Settings { get; set; }
    }
}