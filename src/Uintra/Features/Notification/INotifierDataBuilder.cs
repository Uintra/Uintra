using System;
using System.Threading.Tasks;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Member.Abstractions;
using Uintra.Features.Comments.Models;
using Uintra.Features.Notification.Entities.Base;

namespace Uintra.Features.Notification
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
