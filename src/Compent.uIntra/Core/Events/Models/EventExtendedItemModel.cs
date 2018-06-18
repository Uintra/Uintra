using Compent.uIntra.Core.Activity.Models;
using uIntra.Events;
using uIntra.Likes;

namespace Compent.uIntra.Core.Events
{
    public class EventExtendedItemModel : EventItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public bool IsSubscribed { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}