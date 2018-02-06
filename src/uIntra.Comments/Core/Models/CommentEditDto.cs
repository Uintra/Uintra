using System;

namespace uIntra.Comments
{
    public class CommentEditDto
    {
        public Guid Id { get; }
        public string Text { get; }
        public int? LinkPreviewId { get; }

        public CommentEditDto(Guid id, string text, int? linkPreviewId)
        {
            Id = id;
            Text = text;
            LinkPreviewId = linkPreviewId;
        }
    }
}
