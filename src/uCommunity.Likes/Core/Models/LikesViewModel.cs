using System;
using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Likes
{
    public class LikesViewModel
    {
        public Guid UserId { get; set; }

        public Guid ActivityId { get; set; }

        public int Count { get; set; }

        public bool CanAddLike { get; set; }

        public IEnumerable<string> Users { get; set; }

        public LikesViewModel()
        {
            Users = Enumerable.Empty<string>();
        }
    }
}