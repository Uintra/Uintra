using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.App_Plugins.Core.Activity;

namespace uCommunity.Likes.App_Plugins.Likes.Models
{
    public class LikesViewModel
    {
        public Guid UserId { get; set; }

        public Guid ActivityId { get; set; }

        public int Count { get; set; }

        public bool CanAddLike { get; set; }

        public bool CanRemoveLike { get; set; }

        public IEnumerable<string> Users { get; set; }

        public IntranetActivityTypeEnum Type { get; set; }

        public LikesViewModel()
        {
            Users = Enumerable.Empty<string>();
        }
    }
}