using System;

namespace uIntra.Comments
{
    public class CommentDto
    {
        public Guid UserId { get; }
        public Guid ActivityId { get; }
        public string Text { get; }
        public Guid? ParentId { get; }
        public int? PreviewId { get; }

        public CommentDto(Guid userId, Guid activityId, string text, Guid? parentId, int? previewId)
        {
            UserId = userId;
            ActivityId = activityId;
            Text = text;
            ParentId = parentId;
            PreviewId = previewId;
        }
    }
}
