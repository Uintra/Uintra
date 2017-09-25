using System;
using uIntra.CentralFeed;

namespace uIntra.Groups
{
    public class GroupActivityTransferCreateModel : ActivityTransferCreateModel
    {
        public Guid GroupId { get; set; }
    }
}