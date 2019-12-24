using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes;

namespace Uintra20.Features.Bulletins.Models
{
    public class SocialExtendedViewModel : SocialViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}