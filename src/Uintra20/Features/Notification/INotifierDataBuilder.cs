using System;
using System.Threading.Tasks;
using Uintra20.Features.Activity.Entities;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.User;

namespace Uintra20.Features.Notification
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
