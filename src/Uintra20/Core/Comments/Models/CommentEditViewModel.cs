using System;
using Uintra20.Core.LinkPreview;

namespace Uintra20.Core.Comments
{
    public class CommentEditViewModel
    {
        public string UpdateElementId { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}