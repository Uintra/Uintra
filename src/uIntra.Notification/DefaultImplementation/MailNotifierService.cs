using System;
using System.Linq;
using EmailWorker.Data.Extensions;
using EmailWorker.Data.Model;
using EmailWorker.Data.Services.Interfaces;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Localization;
using uIntra.Core.TypeProviders;
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
        private readonly IIntranetLocalizationService _intranetLocalizationService;

        public MailNotifierService(
            IEmailService emailService,
            IMailConfiguration mailConfiguration,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetLocalizationService intranetLocalizationService)
        {
            _emailService = emailService;
            _mailConfiguration = mailConfiguration;
            _intranetUserService = intranetUserService;
            _intranetLocalizationService = intranetLocalizationService;
        }

        public NotifierTypeEnum Type => NotifierTypeEnum.EmailNotifier;

        public void Notify(NotifierData data)
        {
            var receivers = _intranetUserService.GetMany(data.ReceiverIds).ToList();
            foreach (var receiverId in data.ReceiverIds)
            {
                var user = receivers.Find(receiver => receiver.Id == receiverId);
                SendMail(data.NotificationType, data.Value, user);
            }
        }

        private void SendMail(IIntranetType notificationType, INotifierDataValue notifierDataValue, IIntranetUser user)
        {
            var recipient = new EmailRecipient { Email = user.Email, Name = user.DisplayedName };

            switch (notificationType.Id)
            {
                case (int)NotificationTypeEnum.Event:
                    SendEventMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.EventUpdated:
                    SendEventUpdatedMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.EventHided:
                    SendEventHidedMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.BeforeStart:
                    SendBeforeStartMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.News:
                    SendNewsMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.Idea:
                    SendIdeaMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    SendActivityLikeAddedMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentAdded:
                    SendCommentAddedMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentEdited:
                    SendCommentEditedMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentReplyed:
                    SendCommentReplyedMail(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentLikeAdded:
                    SendCommentLikeAddedMail(notifierDataValue, recipient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType), "Unknown Notification Type");
            }
        }

        protected virtual void SendEventMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var mail = new EventMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendNewsMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var mail = new NewsMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendIdeaMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var mail = new IdeaMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendEventUpdatedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierData<ActivityNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var mail = new EventUpdatedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                Title = data.Title,
                Type = GetActivityTypeText(data.ActivityType),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendEventHidedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierDataWithNotifier<ActivityNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;
            var notifier = notifierData.Item2;

            var mail = new EventHidedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                Title = data.Title,
                NotifierFullName = notifier.DisplayedName,
                Type = GetActivityTypeText(data.ActivityType),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendBeforeStartMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierData<ActivityReminderDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var mail = new BeforeStartMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                ActivityTitle = data.Title,
                ActivityType = GetActivityTypeText(data.ActivityType),
                StartDate = data.StartDate.ToDateTimeFormat(),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendActivityLikeAddedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierData<LikesNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var mail = new ActivityLikeAddedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                ActivityTitle = data.Title,
                ActivityType = GetActivityTypeText(data.ActivityType),
                CreatedDate = data.CreatedDate.ToDateTimeFormat(),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentAddedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierData<CommentNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var mail = new CommentAddedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                ActivityTitle = GetActivityTypeText(data.ActivityType),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentEditedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierData<CommentNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var mail = new CommentEditedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                ActivityTitle = GetActivityTypeText(data.ActivityType),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentReplyedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var notifierData = GetNotifierData<CommentNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var mail = new CommentReplyedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                ActivityTitle = GetActivityTypeText(data.ActivityType),
                Url = data.Url,
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        protected virtual void SendCommentLikeAddedMail(INotifierDataValue notifierDataValue, EmailRecipient recipient)
        {
            var mail = new CommentLikeAddedMail(_mailConfiguration.MailTemplateXpath)
            {
                To = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            _emailService.AddInMailQueue(mail);
        }

        private Tuple<T, Guid?> GetNotifierData<T>(INotifierDataValue notifierDataValue)
        {
            var data = (T)notifierDataValue;
            var notifier = notifierDataValue as IHaveNotifierId;
            var notifierId = notifier?.NotifierId;

            var result = new Tuple<T, Guid?>(data, notifierId);
            return result;
        }

        private Tuple<T, IIntranetUser> GetNotifierDataWithNotifier<T>(INotifierDataValue notifierDataValue)
        {
            var notifierData = GetNotifierData<T>(notifierDataValue);
            var data = notifierData.Item1;
            var notifierId = notifierData.Item2;
            if (!notifierId.HasValue)
            {
                throw new NullReferenceException("Notifier id have to be presented");
            }

            var notifier = _intranetUserService.Get(notifierId.Value);
            var result = new Tuple<T, IIntranetUser>(data, notifier);

            return result;
        }

        private string GetActivityTypeText(IIntranetType activityType)
        {
            var result = _intranetLocalizationService.Translate(activityType.Id.GetLocalizeKey());
            return result;
        }
    }
}
