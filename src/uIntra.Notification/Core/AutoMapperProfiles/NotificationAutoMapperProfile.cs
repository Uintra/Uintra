using System.Web;
using System.Web.Helpers;
using AutoMapper;
using Uintra.Core.Extensions;

namespace Uintra.Notification
{
    public class NotificationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Notification, NotificationViewModel>()
                .ForMember(d => d.Notifier, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToDateTimeFormat()))
                .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)))
                .AfterMap((src, dst) =>
                {
                    var notificationTypeProvider = HttpContext.Current.GetService<INotificationTypeProvider>();
                    dst.Type = notificationTypeProvider[src.Type];
                });

            Mapper.CreateMap<Notification, PopupNotificationViewModel>()
                .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)));

            Mapper.CreateMap<NotifierSettingSaveModel<EmailNotifierTemplate>, NotifierSettingModel<EmailNotifierTemplate>>()
                .ForMember(d => d.NotificationType, o => o.Ignore())
                .ForMember(d => d.NotificationTypeName, o => o.Ignore())
                .ForMember(d => d.NotifierType, o => o.Ignore())
                .ForMember(d => d.ActivityType, o => o.Ignore())
                .ForMember(d => d.ActivityTypeName, o => o.Ignore());

            Mapper.CreateMap<NotifierSettingSaveModel<UiNotifierTemplate>, NotifierSettingModel<UiNotifierTemplate>>()
                .ForMember(d => d.NotificationType, o => o.Ignore())
                .ForMember(d => d.NotificationTypeName, o => o.Ignore())
                .ForMember(d => d.NotifierType, o => o.Ignore())
                .ForMember(d => d.ActivityType, o => o.Ignore())
                .ForMember(d => d.ActivityTypeName, o => o.Ignore());
        }
    }
}