using System;
using System.Collections.Generic;

namespace uCommunity.Likes
{
    public interface ILikeableService
    {
        void Add(Guid userId, Guid activityId);

        void Remove(Guid userId, Guid activityId);

        IEnumerable<LikeModel> GetLikes(Guid activityId);
    }
}
