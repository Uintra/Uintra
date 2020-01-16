using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Likes.Models
{
    public class LikesViewModel
    {
        public Guid MemberId { get; set; }

        public Guid EntityId { get; set; }

        public int Count { get; set; }

        public bool LikedByCurrentUser { get; set; }

        public IEnumerable<string> Users { get; set; } = Enumerable.Empty<string>();

        public bool IsReadOnly { get; set; }

        public bool ShowTitle { get; set; }
    }
}