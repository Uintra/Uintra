using uCommunity.Comments;
using uCommunity.Events;
using uCommunity.Likes;
using uCommunity.Subscribe;

namespace Compent.uCommunity.Core.Events
{
    public class IntranetEventViewModel : EventViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public ISubscribable SubscribeInfo { get; set; }
    }
}