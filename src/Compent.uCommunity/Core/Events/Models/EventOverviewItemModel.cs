using uCommunity.Events;
using uCommunity.Likes;
using uCommunity.Subscribe;

namespace Compent.uCommunity.Core.Events
{
    public class EventOverviewItemModel : EventsOverviewItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ISubscribable SubscribeInfo { get; set; }
    }
}