using System;

namespace Uintra20.Features.Comments.Models
{
    public class CommentCreateDto
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid ActivityId { get; }
        public string Text { get; }
        public Guid? ParentId { get; }
        public int? LinkPreviewId { get; }

        public CommentCreateDto(Guid id, Guid userId, Guid activityId, string text, Guid? parentId, int? linkPreviewId)
        {
            Id = id;
            UserId = userId;
            ActivityId = activityId;
            Text = text;
            ParentId = parentId;
            LinkPreviewId = linkPreviewId;
        }
    }
}