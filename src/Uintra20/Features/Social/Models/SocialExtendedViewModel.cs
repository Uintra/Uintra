using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes;

namespace Uintra20.Features.Social.Models
{
    public class SocialExtendedViewModel : SocialViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}