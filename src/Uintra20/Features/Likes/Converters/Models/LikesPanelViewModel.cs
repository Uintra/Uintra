using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;

namespace Uintra20.Features.Likes.Converters.Models
{
    public class LikesPanelViewModel : NodeViewModel
    {
        public Guid MemberId { get; set; }
        public Guid EntityId { get; set; }
        public int Count { get; set; }
        public bool CanAddLike { get; set; }
        public IEnumerable<string> Users { get; set; } = Enumerable.Empty<string>();
        public bool IsReadOnly { get; set; }
        public bool ShowTitle { get; set; }
    }
}