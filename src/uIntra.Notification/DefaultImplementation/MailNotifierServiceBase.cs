using System;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Localization;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.MailModels;

namespace uIntra.Notification
{
    public class MailNotifierServiceBase : INotifierService
    {
        private readonly IMailService _mailService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetLocalizationService _intranetLocalizationService;

        protected MailNotifierServiceBase(
            IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetLocalizationService intranetLocalizationService)
        {
            _mailService = mailService;
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
                SendMail(data.NotificationType, data.NotificationType, data.Value, user);
            }
        }

        protected virtual void SendMail(IIntranetType activityType, IIntranetType notificationType, INotifierDataValue notifierDataValue, IIntranetUser user)
        {
            var recipient = new MailRecipient { Email = user.Email, Name = user.DisplayedName };

            MailBase mail;

            switch (notificationType.Id)
            {
                case (int)NotificationTypeEnum.Event:
                    mail = GetEventMail<EventMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.News:
                    mail = GetNewsMail<NewsMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.Idea:
                    mail = GetIdeaMail<IdeaMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.EventUpdated:
                    mail = GetEventUpdatedMail<EventUpdatedMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.EventHided:
                    mail = GetEventHidedMail<EventHidedMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.BeforeStart:
                    mail = GetBeforeStartMail<BeforeStartMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    mail = GetActivityLikeAddedMail<ActivityLikeAddedMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentAdded:
                    mail = GetCommentAddedMail<CommentAddedMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentEdited:
                    mail = GetCommentEditedMail<CommentEditedMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentReplied:
                    mail = GetCommentRepliedMail<CommentRepliedMailBase>(notifierDataValue, recipient);
                    break;
                case (int)NotificationTypeEnum.CommentLikeAdded:
                    mail = GetCommentLikeAddedMail<CommentLikeAddedMailBase>(notifierDataValue, recipient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType), "Unknown Notification Type");
            }



            _mailService.Send(mail);
        }

        protected virtual T GetEventMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : EventMailBase, new()
        {
            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetNewsMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : NewsMailBase, new()
        {
            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetIdeaMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : IdeaMailBase, new()
        {
            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetEventUpdatedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : EventUpdatedMailBase, new()
        {
            var notifierData = GetNotifierData<ActivityNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                Title = data.Title,
                Type = GetActivityTypeText(data.NotificationType),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetEventHidedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : EventHidedMailBase, new()
        {
            var notifierData = GetNotifierDataWithNotifier<ActivityNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;
            var notifier = notifierData.Item2;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                Title = data.Title,
                NotifierFullName = notifier.DisplayedName,
                Type = GetActivityTypeText(data.NotificationType),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetBeforeStartMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : BeforeStartMailBase, new()
        {
            var notifierData = GetNotifierData<ActivityReminderDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                ActivityTitle = data.Title,
                ActivityType = GetActivityTypeText(data.NotificationType),
                StartDate = data.StartDate.ToDateTimeFormat(),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetActivityLikeAddedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : ActivityLikeAddedMailBase, new()
        {
            var notifierData = GetNotifierData<LikesNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                ActivityTitle = data.Title,
                ActivityType = GetActivityTypeText(data.NotificationType),
                CreatedDate = data.CreatedDate.ToDateTimeFormat(),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };
            return result;
        }

        protected virtual T GetCommentAddedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : CommentAddedMailBase, new()
        {
            var notifierData = GetNotifierData<CommentNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                ActivityTitle = GetActivityTypeText(data.NotificationType),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetCommentEditedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : CommentEditedMailBase, new()
        {
            var notifierData = GetNotifierData<CommentNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                ActivityTitle = GetActivityTypeText(data.NotificationType),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetCommentRepliedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : CommentRepliedMailBase, new()
        {
            var notifierData = GetNotifierData<CommentNotifierDataModel>(notifierDataValue);
            var data = notifierData.Item1;

            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                ActivityTitle = GetActivityTypeText(data.NotificationType),
                Url = data.Url.ToAbsoluteUrl(),
                FullName = recipient.Name
            };

            return result;
        }

        protected virtual T GetCommentLikeAddedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
            where T : CommentLikeAddedMailBase, new()
        {
            var result = new T
            {
                Recipients = recipient.ToListOfOne(),
                FullName = recipient.Name
            };

            return result;
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
            var result = _intranetLocalizationService.Translate(activityType.Name);
            return result;
        }
    }
}
