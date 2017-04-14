using System;

namespace uCommunity.Comments.Core.Events
{
    public class CommentEdited
    {
        public CommentEdited(Guid id, Guid activityId)
        {
            Id = id;
            ActivityId = activityId;
        }

        public Guid Id { get; private set; }
        public Guid ActivityId { get; private set; }
    }
}
