using uIntra.Comments;
using uIntra.Events;
using uIntra.Likes;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Events
{
    public class EventExtendedViewModel : EventViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public ISubscribable SubscribeInfo { get; set; }
    }
}