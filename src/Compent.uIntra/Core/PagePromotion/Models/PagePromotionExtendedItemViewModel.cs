using Uintra.Comments;
using Uintra.Core.PagePromotion;
using Uintra.Likes;

namespace Compent.Uintra.Core.PagePromotion.Models
{
    public class PagePromotionExtendedItemViewModel : PagePromotionItemViewModel
    {
        public ILikeable LikesInfo { get; set; }

        public ICommentable CommentsInfo { get; set; }

        public bool Likeable { get; set; }

        public bool Commentable { get; set; }
    }
}