using uIntra.Core.User;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public interface INotificationModelMapper<in TTemplate, out TNotificationModel>
        where TNotificationModel : INotificationMessage where TTemplate : INotifierTemplate
    {
        TNotificationModel Map(INotifierDataValue notifierData, TTemplate template, IIntranetUser receiver);
    }
}
