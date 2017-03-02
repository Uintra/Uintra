using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Comments.App_Plugins.Comments.Models
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