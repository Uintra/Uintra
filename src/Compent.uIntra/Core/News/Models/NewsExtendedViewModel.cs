using Uintra.Comments;
using Uintra.Likes;
using Uintra.News;

namespace Compent.Uintra.Core.News.Models
{
    public class NewsExtendedViewModel : NewsViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}