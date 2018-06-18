using System;

namespace Uintra.Comments
{
    public class CommentCreateDto
    {
        public Guid UserId { get; }
        public Guid ActivityId { get; }
        public string Text { get; }
        public Guid? ParentId { get; }
        public int? LinkPreviewId { get; }

        public CommentCreateDto(Guid userId, Guid activityId, string text, Guid? parentId, int? linkPreviewId)
        {
            UserId = userId;
            ActivityId = activityId;
            Text = text;
            ParentId = parentId;
            LinkPreviewId = linkPreviewId;
        }
    }
}
