using Uintra20.Core.Activity;
using Uintra20.Core.Comments;
using Uintra20.Core.Likes;

namespace Uintra20.Core.Bulletins
{
    public class BulletinExtendedItemViewModel : BulletinItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}