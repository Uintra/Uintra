using System;
using System.Collections.Generic;

namespace uIntra.Likes
{
    public interface ILikesService
    {
        IEnumerable<Like> Get(Guid entityId);

        int GetCount(Guid entityId);

        void Add(Guid userId, Guid entityId);

        void Remove(Guid userId, Guid entityId);

        bool CanAdd(Guid userId, Guid entityId);

        void FillLikes(ILikeable entity);

        IEnumerable<LikeModel> GetLikeModels(Guid entityId);
    }
}
