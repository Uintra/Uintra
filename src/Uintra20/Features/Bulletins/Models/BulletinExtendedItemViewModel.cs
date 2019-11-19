using Uintra20.Features.Activity.Models.Headers;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes;

namespace Uintra20.Features.Bulletins.Models
{
    public class BulletinExtendedItemViewModel : BulletinItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}