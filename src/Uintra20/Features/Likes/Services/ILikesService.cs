using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Sql;

namespace Uintra20.Features.Likes.Services
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


        Task<IEnumerable<Like>> GetAsync(Guid entityId);
        Task<int> GetCountAsync(Guid entityId);
        Task AddAsync(Guid userId, Guid entityId);
        Task RemoveAsync(Guid userId, Guid entityId);
        Task<bool> CanAddAsync(Guid userId, Guid entityId);
        Task FillLikesAsync(ILikeable entity);
        Task<IEnumerable<LikeModel>> GetLikeModelsAsync(Guid entityId);
    }
}
