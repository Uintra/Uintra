using System;

namespace uCommunity.Likes
{
    public interface ILikeableService
    {
        ILikeable AddLike(Guid userId, Guid activityId);

        ILikeable RemoveLike(Guid userId, Guid activityId);
    }
}
