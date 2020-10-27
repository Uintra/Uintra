using Uintra.Core.Activity.Models.Headers;
using Uintra.Features.Comments.Services;
using Uintra.Features.Likes;

namespace Uintra.Features.Social.Models
{
    public class SocialExtendedItemViewModel : SocialItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}