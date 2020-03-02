using System;
using System.Threading.Tasks;
using UBaseline.Shared.Node;
using Uintra20.Core.Activity.Entities;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities;

namespace Uintra20.Infrastructure.Helpers
{
    public interface INotifierDataHelper
    {
        ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        ActivityReminderDataModel GetActivityReminderDataModel(IIntranetActivity activity, Enum notificationType);
        CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId);
        CommentNotifierDataModel GetCommentNotifierDataModel(INodeModel content, CommentModel comment, Enum notificationType, Guid notifierId);
        LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        GroupInvitationDataModel GetGroupInvitationDataModel(NotificationTypeEnum notificationType, Guid groupId, Guid notifierId, Guid receiverId);

        Task<ActivityNotifierDataModel> GetActivityNotifierDataModelAsync(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        Task<ActivityReminderDataModel> GetActivityReminderDataModelAsync(IIntranetActivity activity, Enum notificationType);
        Task<CommentNotifierDataModel> GetCommentNotifierDataModelAsync(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId);
        Task<LikesNotifierDataModel> GetLikesNotifierDataModelAsync(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        Task<GroupInvitationDataModel> GetGroupInvitationDataModelAsync(NotificationTypeEnum notificationType, Guid groupId, Guid notifierId, Guid receiverId);
    }
}
