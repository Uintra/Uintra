using System.Collections.Generic;
using System.Linq;

namespace Uintra.Comments
{
    public class CommentsListModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public CommentsListModel()
        {
            Comments = Enumerable.Empty<CommentViewModel>();
        }
    }
}