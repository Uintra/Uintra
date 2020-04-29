using AutoMapper;
using System.Web;
using Compent.LinkPreview.HttpClient.Extensions;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Json;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.Configuration;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.ViewModel;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification.AutoMapperProfiles
{
    public class NotificationAutoMapperProfile : Profile
    {
        public NotificationAutoMapperProfile()
        {
            CreateMap<Sql.Notification, NotificationViewModel>()
                .ForMember(d => d.Notifier, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToDateTimeFormat()))
                .ForMember(d => d.Value, o => o.MapFrom(s => System.Web.Helpers.Json.Decode(s.Value)))
                .AfterMap((src, dst) =>
                {
                    var msg = src.Value.Deserialize<UiNotificationMessage>();
                    dst.Notifier = new MemberViewModel()
                    {
                        Photo = msg.NotifierPhotoUrl
                    };
                    var notificationTypeProvider = HttpContext.Current.GetService<INotificationTypeProvider>();
                    dst.Type = notificationTypeProvider[src.Type];
                });

            CreateMap<Sql.Notification, JsonNotification>()
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.DesktopMessage, o => o.Ignore())
                .ForMember(d => d.DesktopTitle, o => o.Ignore())
                .ForMember(d => d.IsDesktopNotificationEnabled, o => o.Ignore())
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToDateTimeFormat()))
                .ForMember(d => d.Value, o => o.MapFrom(s => System.Web.Helpers.Json.Decode(s.Value)))
                .ForMember(d => d.NotifierId, o => o.Ignore())
                .ForMember(d => d.NotifierPhoto, o => o.Ignore())
                .ForMember(d => d.NotifierDisplayedName, o => o.Ignore())
                .ForMember(d => d.Message, o => o.Ignore())
                .ForMember(d => d.Url, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    var notificationTypeProvider = HttpContext.Current.GetService<INotificationTypeProvider>();
                    dst.Type = notificationTypeProvider[src.Type];
                    dst.Message = (string) dst.Value.message;
                    dst.Url = (string) dst.Value.url;
                    dst.DesktopMessage = (string) dst.Value.desktopMessage;
                    dst.DesktopTitle = (string) dst.Value.desktopTitle;
                    dst.IsDesktopNotificationEnabled = (bool) dst.Value.isDesktopNotificationEnabled;
                });

            CreateMap<Sql.Notification, PopupNotificationViewModel>()
                .ForMember(d => d.Value, o => o.MapFrom(s => System.Web.Helpers.Json.Decode(s.Value)));

            CreateMap<NotifierSettingSaveModel<EmailNotifierTemplate>, NotifierSettingModel<EmailNotifierTemplate>>()
                .ForMember(d => d.NotificationType, o => o.Ignore())
                .ForMember(d => d.NotificationTypeName, o => o.Ignore())
                .ForMember(d => d.NotifierType, o => o.Ignore())
                .ForMember(d => d.ActivityType, o => o.Ignore())
                .ForMember(d => d.ActivityTypeName, o => o.Ignore());


            CreateMap<NotifierSettingSaveModel<UiNotifierTemplate>, NotifierSettingModel<UiNotifierTemplate>>()
                .ForMember(d => d.NotificationType, o => o.Ignore())
                .ForMember(d => d.NotificationTypeName, o => o.Ignore())
                .ForMember(d => d.NotifierType, o => o.Ignore())
                .ForMember(d => d.ActivityType, o => o.Ignore())
                .ForMember(d => d.ActivityTypeName, o => o.Ignore());

            CreateMap<NotifierSettingSaveModel<PopupNotifierTemplate>, NotifierSettingModel<PopupNotifierTemplate>>()
                .ForMember(d => d.NotificationType, o => o.Ignore())
                .ForMember(d => d.NotificationTypeName, o => o.Ignore())
                .ForMember(d => d.NotifierType, o => o.Ignore())
                .ForMember(d => d.ActivityType, o => o.Ignore())
                .ForMember(d => d.ActivityTypeName, o => o.Ignore());

            CreateMap<NotifierSettingSaveModel<DesktopNotifierTemplate>, NotifierSettingModel<DesktopNotifierTemplate>
                >()
                .ForMember(d => d.NotificationType, o => o.Ignore())
                .ForMember(d => d.NotificationTypeName, o => o.Ignore())
                .ForMember(d => d.NotifierType, o => o.Ignore())
                .ForMember(d => d.ActivityType, o => o.Ignore())
                .ForMember(d => d.ActivityTypeName, o => o.Ignore());
        }
    }
}