using Compent.Shared.Extensions.Bcl;
using EmailWorker.Data.Infrastructure;
using System;
using System.Threading.Tasks;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Helpers;
using Umbraco.Web;

namespace Uintra20.Features.ContentPage.Services
{
    public class ContentPageNotificationService : INotifyableService
    {
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly INotificationsService _notificationsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ICommentsService _commentsService;
        private readonly INotifierDataBuilder _notifierDataBuilder;
        private readonly UmbracoHelper _umbracoHelper;

        public ContentPageNotificationService(
            INotifierDataHelper notifierDataHelper,
            IUBaselineRequestContext requestContext,
            INotificationsService notificationsService,
            IIntranetMemberService<IntranetMember> memberService,
            ICommentsService commentsService,
            INotifierDataBuilder notifierDataBuilder,
            UmbracoHelper umbracoHelper)
        {
            _notifierDataHelper = notifierDataHelper;
            _requestContext = requestContext;
            _notificationsService = notificationsService;
            _memberService = memberService;
            _commentsService = commentsService;
            _notifierDataBuilder = notifierDataBuilder;
            _umbracoHelper = umbracoHelper;
        }
        public Enum Type => IntranetActivityTypeEnum.ContentPage;
        public void Notify(Guid entityId, Enum notificationType)
        {
            var isCorrectNotification = notificationType.In(
                NotificationTypeEnum.CommentLikeAdded,
                NotificationTypeEnum.CommentReplied);

            if (!isCorrectNotification) 
                return;

            var notifierData = GetNotifierData(entityId, notificationType);
            _notificationsService.ProcessNotification(notifierData);
        }

        public async Task NotifyAsync(Guid entityId, Enum notificationType)
        {
            var isCorrectNotification = notificationType.In(
                NotificationTypeEnum.CommentLikeAdded,
                NotificationTypeEnum.CommentReplied);

            if (!isCorrectNotification)
                return;

            var notifierData = await GetNotifierDataAsync(entityId, notificationType);
            await _notificationsService.ProcessNotificationAsync(notifierData);
        }

        private NotifierData GetNotifierData(Guid entityId, Enum notificationType)
        {
            var currentMember = _memberService.GetCurrentMember();

            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = Type,
            };

            switch (notificationType)
            {
                case NotificationTypeEnum.CommentLikeAdded:
                case NotificationTypeEnum.CommentReplied:
                {
                    var comment = _commentsService.Get(entityId);
                    data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                    var currentContentPage = _umbracoHelper.Content(_requestContext.Node.Id);
                    data.Value = _notifierDataHelper.GetCommentNotifierDataModel(currentContentPage, comment, notificationType, currentMember.Id);
                }
                    break;
                default: throw new InvalidOperationException();
            }
            return data;
        }

        private async Task<NotifierData> GetNotifierDataAsync(Guid entityId, Enum notificationType)
        {
            var currentMember = await _memberService.GetCurrentMemberAsync();

            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = Type,
            };

            switch (notificationType)
            {
                case NotificationTypeEnum.CommentLikeAdded:
                case NotificationTypeEnum.CommentReplied:
                {
                    var comment = await _commentsService.GetAsync(entityId);
                    data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                    var currentContentPage = _umbracoHelper.Content(entityId);
                    data.Value = _notifierDataHelper.GetCommentNotifierDataModel(currentContentPage, comment, notificationType, currentMember.Id);
                }
                    break;
                default: throw new InvalidOperationException();
            }
            return data;
        }
    }
}