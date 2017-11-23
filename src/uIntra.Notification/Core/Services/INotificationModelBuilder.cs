using uIntra.Notification.Base;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationModelMapper<out TNotificationModel> where TNotificationModel : INotificationMessage
    {
        TNotificationModel Map(NotifierData notifierData, UiNotifierTemplate template);
    }
}
