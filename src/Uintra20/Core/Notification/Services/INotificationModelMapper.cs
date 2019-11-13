using System.Threading.Tasks;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.User;

namespace Uintra20.Core.Notification
{
    public interface INotificationModelMapper<in TTemplate, TNotificationModel>
        where TNotificationModel : INotificationMessage where TTemplate : INotifierTemplate
    {
        TNotificationModel Map(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
        Task<TNotificationModel> MapAsync(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
    }
}
