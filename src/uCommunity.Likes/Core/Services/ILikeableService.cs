using System;

namespace uCommunity.Likes
{
    public interface ILikeableService
    {
        ILikeable Add(Guid userId, Guid activityId);

        ILikeable Remove(Guid userId, Guid activityId);
    }
}
