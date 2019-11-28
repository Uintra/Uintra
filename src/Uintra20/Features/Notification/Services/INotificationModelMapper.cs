using System.Threading.Tasks;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;

namespace Uintra20.Features.Notification.Services
{
    public interface INotificationModelMapper<in TTemplate, TNotificationModel>
        where TNotificationModel : INotificationMessage where TTemplate : INotifierTemplate
    {
        TNotificationModel Map(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
        //Task<TNotificationModel> MapAsync(INotifierDataValue notifierData, TTemplate template, IIntranetMember receiver);
    }
}
