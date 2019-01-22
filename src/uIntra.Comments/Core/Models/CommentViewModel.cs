using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.LinkPreview;
using Uintra.Core.User;

namespace Uintra.Comments
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public MemberViewModel Creator { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string Text { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
        
        public bool IsReply { get; set; }

        public string ElementOverviewId { get; set; }

        public string CommentViewId { get; set; }

        public IEnumerable<CommentViewModel> Replies { get; set; } = Enumerable.Empty<CommentViewModel>();

        public string CreatorProfileUrl { get; set; }
        
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}