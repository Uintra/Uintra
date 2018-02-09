using Uintra.Bulletins;
using Uintra.Comments;
using Uintra.Likes;

namespace Compent.Uintra.Core.Bulletins
{
    public class BulletinExtendedViewModel : BulletinViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}