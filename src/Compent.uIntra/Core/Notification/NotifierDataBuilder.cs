using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Compent.Uintra.Core.Helpers;
using LanguageExt;
using Uintra.Comments;
using Uintra.Core.Activity;
using Uintra.Core.PagePromotion;
using Uintra.Core.User;
using Uintra.Notification.Base;
using Uintra.Subscribe;
using static LanguageExt.Prelude;
using static Uintra.Notification.Configuration.NotificationTypeEnum;

namespace Compent.Uintra.Core.Notification
{
    public class NotifierDataBuilder: INotifierDataBuilder
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotifierDataHelper _notifierDataHelper;


        public NotifierDataBuilder(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotifierDataHelper notifierDataHelper)
        {
            _intranetUserService = intranetUserService;
            _notifierDataHelper = notifierDataHelper;
        }

        public NotifierData GetNotifierData<TActivity>(TActivity activity, Enum notificationType) 
            where TActivity : IIntranetActivity, IHaveOwner
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                ReceiverIds = ReceiverIds(activity, notificationType)
            };

            switch (notificationType)
            {
                case ActivityLikeAdded:
                    data.Value = _notifierDataHelper.GetLikesNotifierDataModel(activity, notificationType, currentUser.Id);
                    break;

                case BeforeStart:
                    data.Value = _notifierDataHelper.GetActivityReminderDataModel(activity, notificationType);
                    break;

                case EventHidden:
                case EventUpdated:
                    data.Value = _notifierDataHelper.GetActivityNotifierDataModel(activity, notificationType, currentUser.Id);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return data;
        }

        public NotifierData GetNotifierData<TActivity>(CommentModel comment, TActivity activity, Enum notificationType)
            where TActivity : IIntranetActivity, IHaveOwner
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                ReceiverIds = ReceiverIds(comment, activity, notificationType, currentUser),
                Value = _notifierDataHelper.GetCommentNotifierDataModel(activity, comment, notificationType, currentUser.Id)
            };

            return data;
        }

        private static IEnumerable<Guid> ReceiverIds<TActivity>(TActivity activity, Enum notificationType)
            where TActivity : IIntranetActivity, IHaveOwner
        {
            switch (notificationType)
            {
                case Enum type when type.In(BeforeStart, EventHidden, EventUpdated) && activity is ISubscribable subscribable:
                    return GetNotifiedSubscribers(subscribable);

                case ActivityLikeAdded:
                    return OwnerId(activity);

                default:
                    throw new InvalidOperationException();
            }
        }

        private static IEnumerable<Guid> ReceiverIds<TActivity>(
            CommentModel comment,
            TActivity activity,
            Enum notificationType,
            IIntranetUser currentUser) 
            where TActivity : IIntranetActivity, IHaveOwner
        {
            switch (notificationType)
            {
                case CommentAdded when activity is ISubscribable subscribable:
                    return GetNotifiedSubscribers(subscribable).Concat(OwnerId(comment)).Distinct();

                case CommentAdded:
                    return OwnerId(activity);

                case CommentEdited:
                    return OwnerId(activity);

                case CommentLikeAdded when activity is PagePromotionBase:
                    return OwnerId(comment);

                case CommentLikeAdded:
                    return currentUser.Id == comment.UserId ? List<Guid>() : OwnerId(comment);

                case CommentReplied:
                    return OwnerId(comment);

                default:
                    throw new InvalidOperationException();
            }
        }

        private static Lst<Guid> OwnerId(IHaveOwner haveOwner) =>
            List(haveOwner.OwnerId);

        private static Lst<Guid> OwnerId(CommentModel comment) =>
            List(comment.UserId);

        private static IEnumerable<Guid> GetNotifiedSubscribers(ISubscribable subscribable) =>
            subscribable.Subscribers
                .Where(s => !s.IsNotificationDisabled)
                .Select(s => s.UserId);
    }
}