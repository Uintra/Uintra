using System;

namespace Uintra.CentralFeed
{
    public class ActivityTransferCreateModel
    {
        public Enum Type { get; set; }
        public Guid OwnerId { get; set; }
    }
}