using Compent.uIntra.Core.Activity.Models;
using uIntra.Bulletins;
using uIntra.Comments;
using uIntra.Likes;

namespace Compent.uIntra.Core.Bulletins
{
    public class BulletinExtendedItemViewModel : BulletinItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}