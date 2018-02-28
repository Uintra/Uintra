using Compent.Uintra.Core.Activity.Models;
using Uintra.Events;
using Uintra.Likes;

namespace Compent.Uintra.Core.Events
{
    public class EventExtendedItemModel : EventItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public bool IsSubscribed { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}