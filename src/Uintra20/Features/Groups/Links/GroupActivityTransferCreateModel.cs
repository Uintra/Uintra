using System;
using Uintra20.Features.CentralFeed.Models;

namespace Uintra20.Features.Groups.Links
{
    public class GroupActivityTransferCreateModel : ActivityTransferCreateModel
    {
        public Guid GroupId { get; set; }
    }
}