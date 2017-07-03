using System;
using System.Collections.Generic;
using System.Linq;
using EmailWorker.Data.Model;
using EmailWorker.Data.Services.Interfaces;
using uIntra.Core.User;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.MailModels;

namespace uIntra.Notification
{
    public class MailNotifierService : INotifierService
    {
        private readonly IEmailService _emailService;
        private readonly IMailConfiguration _mailConfiguration;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public MailNotifierService(
            IEmailService emailService,
            IMailConfiguration mailConfiguration,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _emailService = emailService;
            _mailConfiguration = mailConfiguration;
            _intranetUserService = intranetUserService;
        }

        public NotifierTypeEnum Type => NotifierTypeEnum.EmailNotifier;
        public void Notify(NotifierData data)
        {
            switch (data.NotificationType.Id)
            {
                case (int)NotificationTypeEnum.Event:
                    SendEventMail(data);
                    break;
                case (int)NotificationTypeEnum.EventUpdated:
                    SendEventUpdatedMail(data);
                    break;
                case (int)NotificationTypeEnum.EventHided:
                    SendEventHidedMail(data);
                    break;
                case (int)NotificationTypeEnum.BeforeStart:
                    SendBeforeStartMail(data);
                    break;
                case (int)NotificationTypeEnum.News:
                    SendNewsMail(data);
                    break;
                case (int)NotificationTypeEnum.Idea:
                    SendIdeaMail(data);
                    break;
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    SendActivityLikeAddedMail(data);
                    break;
                case (int)NotificationTypeEnum.CommentAdded:
                    SendCommentAddedMail(data);
                    break;
                case (int)NotificationTypeEnum.CommentEdited:
                    SendCommentEditedMail(data);
                    break;
                case (int)NotificationTypeEnum.CommentReplyed:
                    SendCommentReplyedMail(data);
                    break;
                case (int)NotificationTypeEnum.CommentLikeAdded:
                    SendCommentLikeAddedMail(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("NotificationType", "Unknown Notification Type");
            }
        }

        protected virtual void SendEventMail(NotifierData data)
        {
            var mail = new EventMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendEventUpdatedMail(NotifierData data)
        {
            var mail = new EventUpdatedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendEventHidedMail(NotifierData data)
        {
            var mail = new EventHidedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendBeforeStartMail(NotifierData data)
        {
            //var dataModel = data.Value;

            var mail = new BeforeStartMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds),
                //ActivityTitle =  ,
                //FullName = ,
                //ActivityType = ,
                //StartDate = ,
                //Url = 
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendNewsMail(NotifierData data)
        {
            var mail = new NewsMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendIdeaMail(NotifierData data)
        {
            var mail = new IdeaMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendActivityLikeAddedMail(NotifierData data)
        {
            var mail = new ActivityLikeAddedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentAddedMail(NotifierData data)
        {
            var mail = new CommentAddedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentEditedMail(NotifierData data)
        {
            var mail = new CommentEditedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentReplyedMail(NotifierData data)
        {
            var mail = new CommentReplyedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentLikeAddedMail(NotifierData data)
        {
            var mail = new CommentLikeAddedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = GetRecipients(data.ReceiverIds)
            };

            //_emailService.AddInMailQueue(mail);
        }

        private List<EmailRecipient> GetRecipients(IEnumerable<Guid> recipientsIds)
        {
            var result = _intranetUserService.GetMany(recipientsIds)
                .Select(user => new EmailRecipient { Email = user.Email, Name = user.DisplayedName });

            return result.ToList();
        }
    }
}
