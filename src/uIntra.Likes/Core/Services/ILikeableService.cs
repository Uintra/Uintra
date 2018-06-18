using System;
using Uintra.Core.Activity;

namespace Uintra.Likes
{
    public interface ILikeableService : ITypedService
    {
        ILikeable AddLike(Guid userId, Guid activityId);

        ILikeable RemoveLike(Guid userId, Guid activityId);
    }
}
