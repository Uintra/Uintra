using uIntra.Bulletins;
using uIntra.Comments;
using uIntra.Likes;

namespace uIntra.Core.Bulletins
{
    public class BulletinExtendedItemViewModel : BulletinItemViewModel
    {
        public ILikeable LikesInfo { get; set; }

        public ICommentable CommentsInfo { get; set; }
    }
}