using System;
using uIntra.Core.Activity;

namespace uIntra.Likes
{
    public interface ILikeableService : ITypedService
    {
        ILikeable AddLike(Guid userId, Guid activityId);

        ILikeable RemoveLike(Guid userId, Guid activityId);
    }
}
