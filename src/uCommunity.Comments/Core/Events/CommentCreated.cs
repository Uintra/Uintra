using System;

namespace uCommunity.Comments.Core.Events
{
    public class CommentCreated
    {
        public CommentCreated(Guid id, Guid activityId, Guid? parentId)
        {
            Id = id;
            ActivityId = activityId;
            ParentId = parentId;
        }

        public Guid Id { get; private set; }
        public Guid ActivityId { get; private set; }
        public Guid? ParentId { get; private set; }
    }
}
