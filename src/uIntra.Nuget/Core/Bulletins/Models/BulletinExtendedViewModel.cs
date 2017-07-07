using uIntra.Bulletins;
using uIntra.Comments;
using uIntra.Likes;

namespace uIntra.Core.Bulletins
{
    public class BulletinExtendedViewModel : BulletinViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}