using System;

namespace uIntra.CentralFeed
{
    public class ActivityTransferCreateModel
    {
        public Enum Type { get; set; }
        public Guid OwnerId { get; set; }
    }
}