﻿using System;
using System.Threading.Tasks;
using UBaseline.Shared.Node;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Features.Comments.Links;
using Uintra.Features.Comments.Models;
using Uintra.Features.Events.Entities;
using Uintra.Features.Groups.Links;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Notification.Entities;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Infrastructure.Helpers
{
    public class NotifierDataHelper : INotifierDataHelper
    {
        private readonly IActivityLinkService _linkService;
        private readonly ICommentLinkHelper _commentLinkHelper;
        private readonly IGroupService _groupService;
        private readonly IGroupLinkProvider _groupLinkProvider;

        const int MaxTitleLength = 100;

        public NotifierDataHelper(
            IActivityLinkService linkService,
            ICommentLinkHelper commentLinkHelper,
            IGroupService groupService,
            IGroupLinkProvider groupLinkProvider
            )
        {
            _linkService = linkService;
            _commentLinkHelper = commentLinkHelper;
            _groupService = groupService;
            _groupLinkProvider = groupLinkProvider;
        }

        public CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId)
        {
            return new CommentNotifierDataModel
            {
                CommentId = comment.Id,
                NotificationType = notificationType,
                NotifierId = notifierId,
                Title = GetNotifierDataTitle(activity).TrimByWordEnd(MaxTitleLength),
                Url = _commentLinkHelper.GetDetailsUrlWithComment(activity.Id, comment.Id),
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public CommentNotifierDataModel GetCommentNotifierDataModel(INodeModel content, CommentModel comment, Enum notificationType, Guid notifierId)
        {
            return new CommentNotifierDataModel
            {
                CommentId = comment.Id,
                NotificationType = notificationType,
                NotifierId = notifierId,
                Title = content.Name,
                Url = _commentLinkHelper.GetDetailsUrlWithComment(content, comment.Id),
                IsPinned = false,
                IsPinActual = false
            };
        }

        public ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId)
        {
            return new ActivityNotifierDataModel
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                Title = GetNotifierDataTitle(activity).TrimByWordEnd(MaxTitleLength),
                Url = _linkService.GetLinks(activity.Id).Details,
                NotifierId = notifierId,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId)
        {
            return new LikesNotifierDataModel
            {
                Title = GetNotifierDataTitle(activity).TrimByWordEnd(MaxTitleLength),
                NotificationType = notificationType,
                ActivityType = activity.Type,
                NotifierId = notifierId,
                CreatedDate = DateTime.UtcNow,
                Url = _linkService.GetLinks(activity.Id).Details,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public ActivityReminderDataModel GetActivityReminderDataModel(IIntranetActivity activity, Enum activityType)
        {
            var model = new ActivityReminderDataModel
            {
                Url = _linkService.GetLinks(activity.Id).Details,
                Title = activity.Title,
                NotificationType = activityType,
                ActivityType = activity.Type,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };

            if (activity.Type is IntranetActivityTypeEnum.Events)
            {
                var @event = (Event)activity;
                model.StartDate = @event.StartDate;
            }

            return model;
        }

        public GroupInvitationDataModel GetGroupInvitationDataModel(
            NotificationTypeEnum notificationType,
            Guid groupId,
            Guid notifierId,
            Guid receiverId)
        {
            var group = _groupService.Get(groupId);
            var groupRoomLink = _groupLinkProvider.GetGroupRoomLink(groupId);
            var groupInvitationDataModel = new GroupInvitationDataModel
            {
                Url = groupRoomLink,
                Title = group.Title,
                NotificationType = notificationType,
                GroupId = groupId,
                NotifierId = notifierId,
                ReceiverId = receiverId
            };
            return groupInvitationDataModel;
        }
            

        public async Task<ActivityNotifierDataModel> GetActivityNotifierDataModelAsync(IIntranetActivity activity, Enum notificationType, Guid notifierId)
        {
            return new ActivityNotifierDataModel
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                Title = GetNotifierDataTitle(activity).TrimByWordEnd(MaxTitleLength),
                Url = (await _linkService.GetLinksAsync(activity.Id))?.Details,
                NotifierId = notifierId,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public async Task<ActivityReminderDataModel> GetActivityReminderDataModelAsync(IIntranetActivity activity, Enum notificationType)
        {
            var model = new ActivityReminderDataModel
            {
                Url = (await _linkService.GetLinksAsync(activity.Id))?.Details,
                Title = activity.Title,
                NotificationType = notificationType,
                ActivityType = activity.Type,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };

            if (activity.Type is IntranetActivityTypeEnum.Events)
            {
                var @event = (Event)activity;
                model.StartDate = @event.StartDate;
            }

            return model;
        }

        public async Task<CommentNotifierDataModel> GetCommentNotifierDataModelAsync(IIntranetActivity activity, CommentModel comment, Enum notificationType,
            Guid notifierId)
        {
            return new CommentNotifierDataModel
            {
                CommentId = comment.Id,
                NotificationType = notificationType,
                NotifierId = notifierId,
                Title = GetNotifierDataTitle(activity).TrimByWordEnd(MaxTitleLength),
                Url = await _commentLinkHelper.GetDetailsUrlWithCommentAsync(activity.Id, comment.Id),
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public async Task<LikesNotifierDataModel> GetLikesNotifierDataModelAsync(IIntranetActivity activity, Enum notificationType, Guid notifierId)
        {
            return new LikesNotifierDataModel
            {
                Title = GetNotifierDataTitle(activity).TrimByWordEnd(MaxTitleLength),
                NotificationType = notificationType,
                ActivityType = activity.Type,
                NotifierId = notifierId,
                CreatedDate = DateTime.UtcNow,
                Url = (await _linkService.GetLinksAsync(activity.Id))?.Details,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public async Task<GroupInvitationDataModel> GetGroupInvitationDataModelAsync(NotificationTypeEnum notificationType, Guid groupId, Guid notifierId,
            Guid receiverId)
        {
            return new GroupInvitationDataModel
            {
                Url = $"/groups/room?groupId={groupId}".ToLinkModel(),
                Title = (await _groupService.GetAsync(groupId))?.Title,
                NotificationType = notificationType,
                GroupId = groupId,
                NotifierId = notifierId,
                ReceiverId = receiverId
            };
        }

        private static string GetNotifierDataTitle(IIntranetActivity activity)
            => activity.Type is IntranetActivityTypeEnum.Social ? activity.Description.StripHtml() : activity.Title;
    }
}