using System;

namespace uCommunity.Likes
{
    public class AddRemoveLikeModel
    {
        public Guid ActivityId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
