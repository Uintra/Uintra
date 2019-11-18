﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
//using EmailWorker.Data.Extensions;
using LanguageExt;
using Uintra20.Core.Activity;
using Uintra20.Core.Comments;
using Uintra20.Core.Helpers;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Subscribe;
using Uintra20.Core.User;
using static LanguageExt.Prelude;
using static Uintra20.Core.Notification.Configuration.NotificationTypeEnum;

namespace Uintra20.Core.Notification
{
    public class NotifierDataBuilder : INotifierDataBuilder
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly INotifierDataHelper _notifierDataHelper;


        public NotifierDataBuilder(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            INotifierDataHelper notifierDataHelper)
        {
            _intranetMemberService = intranetMemberService;
            _notifierDataHelper = notifierDataHelper;
        }

        public NotifierData GetNotifierData<TActivity>(TActivity activity, Enum notificationType)
            where TActivity : IIntranetActivity, IHaveOwner
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                ReceiverIds = ReceiverIds(activity, notificationType).Except(new []{currentMember.Id}/*.ToEnumerableOfOne()*/)
            };

            switch (notificationType)
            {
                case ActivityLikeAdded:
                    data.Value = _notifierDataHelper.GetLikesNotifierDataModel(activity, notificationType, currentMember.Id);
                    break;

                case BeforeStart:
                    data.Value = _notifierDataHelper.GetActivityReminderDataModel(activity, notificationType);
                    break;

                case EventHidden:
                case EventUpdated:
                    data.Value = _notifierDataHelper.GetActivityNotifierDataModel(activity, notificationType, currentMember.Id);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return data;
        }

        public NotifierData GetNotifierData<TActivity>(CommentModel comment, TActivity activity, Enum notificationType)
            where TActivity : IIntranetActivity, IHaveOwner
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                ReceiverIds = ReceiverIds(comment, activity, notificationType, currentMember).Except(new [] {currentMember.Id}/*.ToEnumerableOfOne()*/),
                Value = _notifierDataHelper.GetCommentNotifierDataModel(activity, comment, notificationType, currentMember.Id)
            };

            return data;
        }

        public async Task<NotifierData> GetNotifierDataAsync<TEntity>(TEntity activity, Enum notificationType) where TEntity : IIntranetActivity, IHaveOwner
        {
            var currentMember = await _intranetMemberService.GetCurrentMemberAsync();
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                ReceiverIds = ReceiverIds(activity, notificationType).Except(new [] {currentMember.Id}/*.ToEnumerableOfOne()*/)
            };

            switch (notificationType)
            {
                case ActivityLikeAdded:
                    data.Value = await _notifierDataHelper.GetLikesNotifierDataModelAsync(activity, notificationType, currentMember.Id);
                    break;

                case BeforeStart:
                    data.Value = await _notifierDataHelper.GetActivityReminderDataModelAsync(activity, notificationType);
                    break;

                case EventHidden:
                case EventUpdated:
                    data.Value = await _notifierDataHelper.GetActivityNotifierDataModelAsync(activity, notificationType, currentMember.Id);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return data;
        }

        public async Task<NotifierData> GetNotifierDataAsync<TEntity>(CommentModel comment, TEntity activity, Enum notificationType) where TEntity : IIntranetActivity, IHaveOwner
        {
            var currentMember = await _intranetMemberService.GetCurrentMemberAsync();
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                ReceiverIds = ReceiverIds(comment, activity, notificationType, currentMember).Except(new []{currentMember.Id}/*.ToEnumerableOfOne()*/),
                Value = await _notifierDataHelper.GetCommentNotifierDataModelAsync(activity, comment, notificationType, currentMember.Id)
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
            IIntranetMember currentMember)
            where TActivity : IIntranetActivity, IHaveOwner
        {
            switch (notificationType)
            {
                case CommentAdded when activity is ISubscribable subscribable:
                    return GetNotifiedSubscribers(subscribable).Concat(OwnerId(activity)).Distinct();

                case CommentAdded:
                    return OwnerId(activity);

                case CommentEdited:
                    return OwnerId(activity);

                //case CommentLikeAdded when activity is PagePromotionBase:
                //    return OwnerId(comment);

                case CommentLikeAdded:
                    return currentMember.Id == comment.UserId ? List<Guid>() : OwnerId(comment);

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