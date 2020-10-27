using Uintra.Features.Comments.Services;
using Uintra.Features.Likes;

namespace Uintra.Features.Social.Models
{
    public class SocialExtendedViewModel : SocialViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}