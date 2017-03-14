using System;
using System.Collections.Generic;

namespace uCommunity.Likes
{
    public interface ILikesService
    {
        IEnumerable<Like> Get(Guid activityId);

        int GetCount(Guid activityId);

        void Add(Guid userId, Guid activityId);

        void Remove(Guid userId, Guid activityId);

        bool CanAdd(Guid userId, Guid activityId);

        bool CanRemove(Guid userId, Guid activityId);

        void FillLikes(ILikeable entity);
    }
}
