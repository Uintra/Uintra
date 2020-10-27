using Uintra.Core.Member.Abstractions;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Models;
using Uintra.Features.Notification.Models.NotifierTemplates;

namespace Uintra.Features.Notification.Services
{
    public interface INotificationModelMapper<in TTemplate, TNotificationModel>
        where TNotificationModel : INotificationMessage where TTemplate : INotifierTemplate
    {
        TNotificationModel Map(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
        //Task<TNotificationModel> MapAsync(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
    }
}
