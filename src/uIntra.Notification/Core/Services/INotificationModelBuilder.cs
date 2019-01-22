using Uintra.Core.User;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public interface INotificationModelMapper<in TTemplate, out TNotificationModel>
        where TNotificationModel : INotificationMessage where TTemplate : INotifierTemplate
    {
        TNotificationModel Map(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
    }
}
