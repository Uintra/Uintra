using System;
using System.Collections.Generic;
using uCommunity.Likes.App_Plugins.Likes.Models;

namespace uCommunity.Likes.App_Plugins.Likes
{
    public interface ILikeableService
    {
        void Add(Guid userId, Guid activityId);

        void Remove(Guid userId, Guid activityId);

        IEnumerable<LikeModel> GetLikes(Guid activityId);
    }
}
