using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using UBaseline.Shared.Title;

namespace Uintra20.Features.Notification.Models
{
    public class NotificationPageModel : NodeModel, ITitleContainer
    {
        public PropertyModel<string> Title { get; set; }
        public PropertyModel<int> NotificationsPopUpCount { get; set; }
    }
}