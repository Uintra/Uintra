using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using UBaseline.Shared.Title;

namespace Uintra.Features.Notification.Models
{
    public class NotificationsPageModel : NodeModel, ITitleContainer
    {
        public PropertyModel<string> Title { get; set; }
        public PropertyModel<int> NotificationsPopUpCount { get; set; }
    }
}