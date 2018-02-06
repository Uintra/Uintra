using System;
using uIntra.Core.LinkPreview;

namespace uIntra.Comments
{
    public class CommentEditViewModel
    {
        public string UpdateElementId { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}