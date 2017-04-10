using uCommunity.Events;
using uCommunity.Likes;

namespace Compent.uCommunity.Core.Events
{
    public class EventOverviewItemModel : EventsOverviewItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
    }
}