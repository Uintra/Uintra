using uCommunity.Events;
using uCommunity.Likes;

namespace Compent.uCommunity.Core.Events
{
    public class EventOverviewItemModel : EventsOverviewItemModelBase
    {
        public ILikeable LikesInfo { get; set; }
    }
}