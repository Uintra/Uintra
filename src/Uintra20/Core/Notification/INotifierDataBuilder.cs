using System;
using System.Threading.Tasks;
using Uintra20.Core.Activity;
using Uintra20.Core.Comments;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.User;

namespace Uintra20.Core.Notification
{
    public interface INotifierDataBuilder
    {
        NotifierData GetNotifierData<TEntity>(TEntity activity, Enum notificationType)
            where TEntity : IIntranetActivity, IHaveOwner;
        NotifierData GetNotifierData<TEntity>(CommentModel comment, TEntity activity, Enum notificationType)
            where TEntity : IIntranetActivity, IHaveOwner;

        Task<NotifierData> GetNotifierDataAsync<TEntity>(TEntity activity, Enum notificationType)
            where TEntity : IIntranetActivity, IHaveOwner;
        Task<NotifierData> GetNotifierDataAsync<TEntity>(CommentModel comment, TEntity activity, Enum notificationType)
            where TEntity : IIntranetActivity, IHaveOwner;
    }
}
