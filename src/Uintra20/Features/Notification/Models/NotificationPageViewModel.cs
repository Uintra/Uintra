using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.Notification.ViewModel;

namespace Uintra20.Features.Notification.Models
{
    public class NotificationPageViewModel : NodeViewModel
    {
        public PropertyViewModel<string> Title { get; set; }
        public PropertyViewModel<int> NotificationsPopUpCount { get; set; }
        public NotificationListViewModel NotificationList { get; set; }
    }
}