using System.Web;
using System.Web.Helpers;
using AutoMapper;
using uIntra.Core.Extensions;

namespace uIntra.Notification
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


            Mapper.CreateMap<NotifierSettingSaveModel<EmailNotifierTemplate>, NotifierSettingModel<EmailNotifierTemplate>>();
            Mapper.CreateMap<NotifierSettingSaveModel<UiNotifierTemplate>, NotifierSettingModel<UiNotifierTemplate>>();
        }
    }
}