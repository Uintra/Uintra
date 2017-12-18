using uIntra.Comments;
using uIntra.Core.PagePromotion;
using uIntra.Likes;

namespace Compent.uIntra.Core.PagePromotion.Models
{
    public class PagePromotionExtendedItemViewModel : PagePromotionItemViewModel
    {
        public ILikeable LikesInfo { get; set; }

        public ICommentable CommentsInfo { get; set; }

        public bool Likeable { get; set; }

        public bool Commentable { get; set; }
    }
}