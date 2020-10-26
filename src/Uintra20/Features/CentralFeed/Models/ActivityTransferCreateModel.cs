using System;

namespace Uintra20.Features.CentralFeed.Models
{
    public class ActivityTransferCreateModel
    {
        public Enum Type { get; set; }
        public Guid OwnerId { get; set; }
    }
}