using System.Collections.Generic;
using System.Linq;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Core.PagePromotion;
using Uintra.Likes;

namespace Compent.Uintra.Core.PagePromotion.Entities
{
    public class PagePromotion : PagePromotionBase, IFeedItem, ICommentable, ILikeable
    {
        public IEnumerable<CommentModel> Comments { get; set; } = Enumerable.Empty<CommentModel>();

        public IEnumerable<LikeModel> Likes { get; set; } = Enumerable.Empty<LikeModel>();

        public bool IsReadOnly { get; set; }

        public bool Likeable { get; set; }

        public bool Commentable { get; set; }
    }
}