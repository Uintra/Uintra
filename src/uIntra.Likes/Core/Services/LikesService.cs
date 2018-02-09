using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Persistence;
using Uintra.Core.User;

namespace Uintra.Likes
{
    public class LikesService : ILikesService
    {
        private readonly ISqlRepository<Like> _likesRepository;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public LikesService(ISqlRepository<Like> likesRepository, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _likesRepository = likesRepository;
            _intranetUserService = intranetUserService;
        }

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
                UserId = el.Item1,
                User = el.Item2
            });

            return result;
        }

        public virtual int GetCount(Guid entityId)
        {
            return (int)_likesRepository.Count(l => l.EntityId == entityId);
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
                    CreatedDate = DateTime.Now
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

        protected virtual IEnumerable<Tuple<Guid, string>> GetManyNames(IEnumerable<Guid> usersIds)
        {
            var users = _intranetUserService.GetMany(usersIds);
            return users.Select(el => new Tuple<Guid, string>(el.Id, el.DisplayedName));
        }
    }
}