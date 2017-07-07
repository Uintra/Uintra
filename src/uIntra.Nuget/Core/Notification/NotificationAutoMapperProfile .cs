using AutoMapper;
using EmailWorker.Data.Model;
using uIntra.Notification.Base;

namespace Compent.uIntra.Core.Notification
{
    public class NotificationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MailRecipient, EmailRecipient>();
            Mapper.CreateMap<MailRecipient, IEmailRecipient>().As<EmailRecipient>();

            Mapper.CreateMap<MailAttachmentFile, EmailAttachmentFile>();
            Mapper.CreateMap<MailAttachmentFile, IEmailAttachmentFile>().As<EmailAttachmentFile>();
        }
    }
}