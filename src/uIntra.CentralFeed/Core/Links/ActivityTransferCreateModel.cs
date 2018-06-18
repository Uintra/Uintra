using System;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class ActivityTransferCreateModel
    {
        public IIntranetType Type { get; set; }
        public Guid OwnerId { get; set; }
    }
}