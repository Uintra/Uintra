using Compent.uIntra.Core.Notification.Mails;
using uIntra.Core.Localization;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Core.Services;

namespace Compent.uIntra.Core.Notification
{
    public class MailNotifierService : MailNotifierServiceBase
    {
        public MailNotifierService(
            IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetLocalizationService intranetLocalizationService) :
            base(mailService, intranetUserService, intranetLocalizationService)
        {
        }

        protected override T GetEventMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetEventMail<EventMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetNewsMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetNewsMail<NewsMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetIdeaMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetIdeaMail<IdeaMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetEventUpdatedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetEventUpdatedMail<EventUpdatedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetEventHidedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetEventHidedMail<EventHidedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetBeforeStartMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetBeforeStartMail<BeforeStartMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetActivityLikeAddedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetActivityLikeAddedMail<ActivityLikeAddedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetCommentAddedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetCommentAddedMail<CommentAddedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetCommentEditedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetCommentEditedMail<CommentEditedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetCommentRepliedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetCommentRepliedMail<CommentRepliedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }

        protected override T GetCommentLikeAddedMail<T>(INotifierDataValue notifierDataValue, MailRecipient recipient)
        {
            var result = base.GetCommentLikeAddedMail<CommentLikeAddedMail>(notifierDataValue, recipient);
            return (T)(object)result;
        }
    }
}