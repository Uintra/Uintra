using Uintra.Comments;
using Uintra.Events;
using Uintra.Likes;
using Uintra.Subscribe;

namespace Compent.Uintra.Core.Events
{
    public class EventExtendedViewModel : EventViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public ISubscribable SubscribeInfo { get; set; }
    }
}