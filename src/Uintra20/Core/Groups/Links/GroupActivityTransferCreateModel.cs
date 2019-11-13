using System;
using Uintra20.Core.CentralFeed;

namespace Uintra20.Core.Groups
{
    public class GroupActivityTransferCreateModel : ActivityTransferCreateModel
    {
        public Guid GroupId { get; set; }
    }
}