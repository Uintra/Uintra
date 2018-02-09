using System;
using Uintra.CentralFeed;

namespace Uintra.Groups
{
    public class GroupActivityTransferCreateModel : ActivityTransferCreateModel
    {
        public Guid GroupId { get; set; }
    }
}