using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Core.Comments
{
    public class CommentsOverviewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public Guid ActivityId { get; set; }

        public string ElementId { get; set; }

        public bool IsReadOnly { get; set; }

        public CommentsOverviewModel()
        {
            Comments = Enumerable.Empty<CommentViewModel>();
        }
    }
}