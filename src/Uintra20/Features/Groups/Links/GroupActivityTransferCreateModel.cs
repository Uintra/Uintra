using System;
using Uintra20.Features.CentralFeed.Links;

namespace Uintra20.Features.Groups.Links
{
    public class GroupActivityTransferCreateModel : ActivityTransferCreateModel
    {
        public Guid GroupId { get; set; }
    }
}