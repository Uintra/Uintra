using System;
using Uintra.Core.LinkPreview;

namespace Uintra.Comments
{
    public class CommentModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ActivityId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Text { get; set; }

        public Guid? ParentId { get; set; }

        public LinkPreview LinkPreview { get; set; }
    }
}
