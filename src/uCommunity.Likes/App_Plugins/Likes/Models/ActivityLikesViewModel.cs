using System;
using System.Collections.Generic;

namespace uCommunity.Likes.App_Plugins.Likes.Models
{
    public class ActivityLikesViewModel
    {
        public Guid UserId { get; set; }

        public Guid ActivityId { get; set; }

        public int Count { get; set; }

        public bool CanAddLike { get; set; }

        public bool CanRemoveLike { get; set; }

        public IEnumerable<string> Users { get; set; }
    }
}