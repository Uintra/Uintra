using System;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Comments.Models
{
    public class CommentEditViewModel
    {
        public string UpdateElementId { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}