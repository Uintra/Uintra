using System;
using System.Threading.Tasks;
using Uintra20.Core.Activity;
using Uintra20.Core.Comments;
using Uintra20.Core.Notification;
using Uintra20.Core.Notification.Configuration;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core.Helpers
{
    public interface INotifierDataHelper
    {
        ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        ActivityReminderDataModel GetActivityReminderDataModel(IIntranetActivity activity, Enum notificationType);
        CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId);
        CommentNotifierDataModel GetCommentNotifierDataModel(IPublishedContent content, CommentModel comment, Enum notificationType, Guid notifierId);
        LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        GroupInvitationDataModel GetGroupInvitationDataModel(NotificationTypeEnum notificationType, Guid groupId, Guid notifierId, Guid receiverId);

        Task<ActivityNotifierDataModel> GetActivityNotifierDataModelAsync(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        Task<ActivityReminderDataModel> GetActivityReminderDataModelAsync(IIntranetActivity activity, Enum notificationType);
        Task<CommentNotifierDataModel> GetCommentNotifierDataModelAsync(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId);
        Task<LikesNotifierDataModel> GetLikesNotifierDataModelAsync(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        Task<GroupInvitationDataModel> GetGroupInvitationDataModelAsync(NotificationTypeEnum notificationType, Guid groupId, Guid notifierId, Guid receiverId);
    }
}
