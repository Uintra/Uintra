using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Sql;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Likes.Services
{
    public class LikesService : ILikesService
    {
        private readonly ISqlRepository<Like> _likesRepository;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public LikesService(ISqlRepository<Like> likesRepository, IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _likesRepository = likesRepository;
            _intranetMemberService = intranetMemberService;
        }

        #region async
        public async Task<IEnumerable<Like>> GetAsync(Guid entityId)
        {
            return await _likesRepository.FindAllAsync(l => l.EntityId == entityId);
        }

        public async Task<int> GetCountAsync(Guid entityId)
        {
            return (int)await _likesRepository.CountAsync(l => l.EntityId == entityId);
        }

        public async Task AddAsync(Guid userId, Guid entityId)
        {
            if (await CanAddAsync(userId, entityId))
            {
                var like = new Like
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow
                };

                await _likesRepository.AddAsync(like);
            }
        }

        public async Task RemoveAsync(Guid userId, Guid entityId)
        {
            await _likesRepository.DeleteAsync(like => like.EntityId == entityId && like.UserId == userId);
        }

        public async Task<bool> CanAddAsync(Guid userId, Guid entityId)
        {
            var exists = await _likesRepository.ExistsAsync(like => like.EntityId == entityId && like.UserId == userId);
            return !exists;
        }

        public async Task FillLikesAsync(ILikeable entity)
        {
            entity.Likes = await GetLikeModelsAsync(entity.Id);
        }

        public async Task<IEnumerable<LikeModel>> GetLikeModelsAsync(Guid entityId)
        {
            var likes = (await GetAsync(entityId)).OrderByDescending(el => el.CreatedDate).ToList();
            if (likes.Count == 0)
            {
                return Enumerable.Empty<LikeModel>();
            }

            var users = await GetManyNamesAsync(likes.Select(el => el.UserId));

            var result = users.Select(el => new LikeModel
            {
                UserId = el.Id,
                User = el.DisplayedName
            });

            return result;
        }

        protected virtual async Task<IEnumerable<(Guid Id, string DisplayedName)>> GetManyNamesAsync(IEnumerable<Guid> usersIds)
        {
            var users = await _intranetMemberService.GetManyAsync(usersIds);
            return users.Select(el => (el.Id, el.DisplayedName));
        }

        #endregion

        #region sync

        public virtual IEnumerable<Like> Get(Guid entityId)
        {
            return _likesRepository.FindAll(l => l.EntityId == entityId);
        }

        public virtual IEnumerable<LikeModel> GetLikeModels(Guid entityId)
        {
            var likes = Get(entityId).OrderByDescending(el => el.CreatedDate).ToList();
            if (likes.Count == 0)
            {
                return Enumerable.Empty<LikeModel>();
            }

            var users = GetManyNames(likes.Select(el => el.UserId));

            var result = users.Select(el => new LikeModel
            {
                UserId = el.Id,
                User = el.DisplayedName
            });

            return result;
        }

        public virtual void Add(Guid userId, Guid entityId)
        {
            if (CanAdd(userId, entityId))
            {
                var like = new Like
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow
                };

                _likesRepository.Add(like);
            }
        }

        public virtual void Remove(Guid userId, Guid entityId)
        {
            _likesRepository.Delete(like => like.EntityId == entityId && like.UserId == userId);
        }

        public virtual bool CanAdd(Guid userId, Guid entityId)
        {
            var exists = _likesRepository.Exists(like => like.EntityId == entityId && like.UserId == userId);
            return !exists;
        }

        public virtual void FillLikes(ILikeable entity)
        {
            entity.Likes = GetLikeModels(entity.Id);
        }

        protected virtual IEnumerable<(Guid Id, string DisplayedName)> GetManyNames(IEnumerable<Guid> usersIds)
        {
            var users = _intranetMemberService.GetMany(usersIds);
            return users.Select(el => (el.Id, el.DisplayedName));
        }
        public virtual int GetCount(Guid entityId)
        {
            return (int)_likesRepository.Count(l => l.EntityId == entityId);
        }

        public virtual bool LikedByCurrentUser(Guid entityId, Guid userId)
        {
            var result = !Get(entityId)
                .Where(l => l.UserId.Equals(userId))
                .IsEmpty();

            return result;
        }

        #endregion

    }
}