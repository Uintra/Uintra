using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Persistence.Sql;
using uCommunity.Core.User;

namespace uCommunity.Likes
{
    public class LikesService : ILikesService
    {
        private readonly ISqlRepository<Like> _likesRepository;
        private readonly IIntranetUserService _intranetUserService;

        public LikesService(ISqlRepository<Like> likesRepository, IIntranetUserService intranetUserService)
        {
            _likesRepository = likesRepository;
            _intranetUserService = intranetUserService;
        }

        public IEnumerable<Like> Get(Guid entityId)
        {
            return _likesRepository.FindAll(l => l.EntityId == entityId);
        }

        public IEnumerable<LikeModel> GetLikeModels(Guid entityId)
        {
            var likes = Get(entityId).OrderByDescending(el => el.CreatedDate).ToList();
            var users = Enumerable.Empty<Tuple<Guid, string>>();
            if (likes.Count != 0)
            {
                users = GetManyNames(likes.Select(el => el.UserId));
            }

            var result = users.Select(el => new LikeModel
            {
                UserId = el.Item1,
                User = el.Item2
            });

            return result;
        }

        public int GetCount(Guid entityId)
        {
            return (int)_likesRepository.Count(l => l.EntityId == entityId);
        }

        public void Add(Guid userId, Guid entityId)
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

        public void Remove(Guid userId, Guid entityId)
        {
            _likesRepository.Delete(like => like.EntityId == entityId && like.UserId == userId);
        }

        public bool CanAdd(Guid userId, Guid entityId)
        {
            var exists = _likesRepository.Exists(like => like.EntityId == entityId && like.UserId == userId);
            return !exists;
        }

        public void FillLikes(ILikeable entity)
        {
            entity.Likes = GetLikeModels(entity.Id);
        }

        private IEnumerable<Tuple<Guid, string>> GetManyNames(IEnumerable<Guid> usersIds)
        {
            var users = _intranetUserService.GetMany(usersIds);
            return users.Select(el => new Tuple<Guid, string>(el.Id, el.DisplayedName));
        }
    }
}