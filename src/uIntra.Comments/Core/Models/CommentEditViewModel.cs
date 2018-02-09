using System;
using Uintra.Core.LinkPreview;

namespace Uintra.Comments
{
    public class CommentEditViewModel
    {
        public string UpdateElementId { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}