using System;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class ActivityTransferCreateModel
    {
        public IIntranetType Type { get; set; }
        public Guid CreatorId { get; set; }
    }
}