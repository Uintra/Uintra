using System;
using Uintra.Features.CentralFeed.Models;

namespace Uintra.Features.Groups.Links
{
    public class GroupActivityTransferCreateModel : ActivityTransferCreateModel
    {
        public Guid GroupId { get; set; }
    }
}