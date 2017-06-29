using uIntra.Core.Activity;
using uIntra.Events;
using uIntra.Likes;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Events
{
    public class EventExtendedItemModel : EventItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ISubscribable SubscribeInfo { get; set; }
    }
}