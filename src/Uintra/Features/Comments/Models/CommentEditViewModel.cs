using System;
using Uintra.Features.LinkPreview.Models;

namespace Uintra.Features.Comments.Models
{
    public class CommentEditViewModel
    {
        public string UpdateElementId { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public LinkPreviewModel LinkPreview { get; set; }
    }
}