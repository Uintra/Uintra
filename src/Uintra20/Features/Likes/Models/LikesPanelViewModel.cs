using System;
using System.Collections.Generic;
using UBaseline.Shared.Node;

namespace Uintra20.Features.Likes.Models
{
    public class LikesPanelViewModel : NodeViewModel
    {
        public Guid EntityId { get; set; }
        public bool LikedByCurrentUser { get; set; }
        public IEnumerable<LikeModel> Likes { get; set; }
        public Enum ActivityType { get; set; }
    }
}