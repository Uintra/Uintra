using Compent.Uintra.Core.Activity.Models;
using Uintra.Bulletins;
using Uintra.Comments;
using Uintra.Likes;

namespace Compent.Uintra.Core.Bulletins
{
    public class BulletinExtendedItemViewModel : BulletinItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}