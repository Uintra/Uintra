using System;
using System.Collections.Generic;
using Uintra.Core.UbaselineModels.RestrictedNode;

namespace Uintra.Features.Likes.Models
{
    public class LikesPanelViewModel : UintraRestrictedNodeViewModel
    {
        public Guid? EntityId { get; set; }
        public bool? LikedByCurrentUser { get; set; }
        public IEnumerable<LikeModel> Likes { get; set; }
        public Enum ActivityType { get; set; }
        public bool IsGroupMember { get; set; }
    }
}