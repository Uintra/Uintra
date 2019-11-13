﻿using AutoMapper;
//using EmailWorker.Data.Model;
using Uintra20.Core.Notification.Base;

namespace Uintra20.Core.Notification
{
    public class NotificationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<Notification, NotificationViewModel>()
            //    .ForMember(d => d.Notifier, o => o.Ignore())
            //    .ForMember(d => d.Type, o => o.Ignore())
            //    .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToDateTimeFormat()))
            //    .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)))
            //    .AfterMap((src, dst) =>
            //    {
            //        var notificationTypeProvider = HttpContext.Current.GetService<INotificationTypeProvider>();
            //        dst.Type = notificationTypeProvider[src.Type];
            //    });

            //Mapper.CreateMap<Notification, JsonNotification>()
            //    .ForMember(d => d.Type, o => o.Ignore())
            //    .ForMember(d => d.DesktopMessage, o => o.Ignore())
            //    .ForMember(d => d.DesktopTitle, o => o.Ignore())
            //    .ForMember(d => d.IsDesktopNotificationEnabled, o => o.Ignore())
            //    .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToDateTimeFormat()))
            //    .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)))
            //    .ForMember(d => d.NotifierId, o => o.Ignore())
            //    .ForMember(d => d.NotifierPhoto, o => o.Ignore())
            //    .ForMember(d => d.NotifierDisplayedName, o => o.Ignore())
            //    .ForMember(d => d.Message, o => o.Ignore())
            //    .ForMember(d => d.Url, o => o.Ignore())
            //    .AfterMap((src, dst) =>
            //    {
            //        var notificationTypeProvider = HttpContext.Current.GetService<INotificationTypeProvider>();
            //        dst.Type = notificationTypeProvider[src.Type];
            //        dst.Message = (string)dst.Value.message;
            //        dst.Url = (string)dst.Value.url;
            //        dst.DesktopMessage = (string)dst.Value.desktopMessage;
            //        dst.DesktopTitle = (string)dst.Value.desktopTitle;
            //        dst.IsDesktopNotificationEnabled = (bool)dst.Value.isDesktopNotificationEnabled;
            //    });

            //Mapper.CreateMap<Notification, PopupNotificationViewModel>()
            //    .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)));

            //Mapper.CreateMap<NotifierSettingSaveModel<EmailNotifierTemplate>, NotifierSettingModel<EmailNotifierTemplate>>()
            //    .ForMember(d => d.NotificationType, o => o.Ignore())
            //    .ForMember(d => d.NotificationTypeName, o => o.Ignore())
            //    .ForMember(d => d.NotifierType, o => o.Ignore())
            //    .ForMember(d => d.ActivityType, o => o.Ignore())
            //    .ForMember(d => d.ActivityTypeName, o => o.Ignore());


            //Mapper.CreateMap<NotifierSettingSaveModel<UiNotifierTemplate>, NotifierSettingModel<UiNotifierTemplate>>()
            //    .ForMember(d => d.NotificationType, o => o.Ignore())
            //    .ForMember(d => d.NotificationTypeName, o => o.Ignore())
            //    .ForMember(d => d.NotifierType, o => o.Ignore())
            //    .ForMember(d => d.ActivityType, o => o.Ignore())
            //    .ForMember(d => d.ActivityTypeName, o => o.Ignore());

            //Mapper.CreateMap<NotifierSettingSaveModel<PopupNotifierTemplate>, NotifierSettingModel<PopupNotifierTemplate>>()
            //    .ForMember(d => d.NotificationType, o => o.Ignore())
            //    .ForMember(d => d.NotificationTypeName, o => o.Ignore())
            //    .ForMember(d => d.NotifierType, o => o.Ignore())
            //    .ForMember(d => d.ActivityType, o => o.Ignore())
            //    .ForMember(d => d.ActivityTypeName, o => o.Ignore());

            //Mapper.CreateMap<NotifierSettingSaveModel<DesktopNotifierTemplate>, NotifierSettingModel<DesktopNotifierTemplate>>()
            //    .ForMember(d => d.NotificationType, o => o.Ignore())
            //    .ForMember(d => d.NotificationTypeName, o => o.Ignore())
            //    .ForMember(d => d.NotifierType, o => o.Ignore())
            //    .ForMember(d => d.ActivityType, o => o.Ignore())
            //    .ForMember(d => d.ActivityTypeName, o => o.Ignore());


            //Mapper.CreateMap<MailRecipient, EmailRecipient>();
            //Mapper.CreateMap<MailRecipient, IEmailRecipient>().As<EmailRecipient>();

            //Mapper.CreateMap<MailAttachmentFile, EmailAttachmentFile>();
            //Mapper.CreateMap<MailAttachmentFile, IEmailAttachmentFile>().As<EmailAttachmentFile>();
        }
    }
}