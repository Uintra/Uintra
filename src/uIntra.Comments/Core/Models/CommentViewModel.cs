using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Comments
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public string CreatorFullName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string Text { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public string Photo { get; set; }

        public bool IsReply { get; set; }

        public string ElementOverviewId { get; set; }

        public string CommentViewId { get; set; }

        public IEnumerable<CommentViewModel> Replies { get; set; }

        public CommentViewModel()
        {
            Replies = Enumerable.Empty<CommentViewModel>();
        }
    }
}