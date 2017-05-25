using uCommunity.Events;
using uCommunity.Likes;
using uCommunity.Subscribe;

namespace Compent.uCommunity.Core.Events
{
    public class EventExtendedItemModel : EventItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ISubscribable SubscribeInfo { get; set; }
    }
}