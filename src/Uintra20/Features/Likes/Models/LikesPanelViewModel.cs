using System;
using System.Collections.Generic;
using UBaseline.Shared.Node;
using Uintra20.Features.Likes.Models;

namespace Uintra20.Features.Likes.Converters.Models
{
    public class LikesPanelViewModel : NodeViewModel
    {
        public Guid EntityId { get; set; }
        public bool LikedByCurrentUser { get; set; }
        public bool IsReadOnly { get; set; }
        public bool ShowTitle { get; set; }
        public IEnumerable<LikeModel> Likes { get; set; }
        public string ActivityType { get; set; }
    }
}