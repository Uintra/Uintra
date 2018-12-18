using System;
using Uintra.Comments;
using Uintra.Core.Activity;
using Uintra.Core.User;
using Uintra.Notification.Base;

namespace Compent.Uintra.Core.Notification
{
    public interface INotifierDataBuilder
    {
        NotifierData GetNotifierData<TEntity>(TEntity activity, Enum notificationType) 
            where TEntity : IIntranetActivity, IHaveOwner;

        NotifierData GetNotifierData<TEntity>(CommentModel comment, TEntity activity, Enum notificationType) 
            where TEntity : IIntranetActivity, IHaveOwner;
    }
}