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

        public IEnumerable<Like> Get(Guid activityId)
        {
            return _likesRepository.FindAll(l => l.ActivityId == activityId);
        }

        public int GetCount(Guid activityId)
        {
            return (int)_likesRepository.Count(l => l.ActivityId == activityId);
        }

        public void Add(Guid userId, Guid activityId)
        {
            var like = new Like
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                UserId = userId,
                CreatedDate = DateTime.Now
            };

            _likesRepository.Add(like);
        }

        public void Remove(Guid userId, Guid activityId)
        {
            _likesRepository.Delete(like => like.ActivityId == activityId && like.UserId == userId);
        }

        public bool CanAdd(Guid userId, Guid activityId)
        {
            var exists = _likesRepository.Exists(like => like.ActivityId == activityId && like.UserId == userId);
            return !exists;
        }

        public void FillLikes(ILikeable entity)
        {
            var likes = Get(entity.Id).OrderByDescending(el => el.CreatedDate).ToList();
            var users = Enumerable.Empty<Tuple<Guid, string>>();
            if (likes.Count != 0)
            {
                users = GetManyNames(likes.Select(el => el.UserId));
            }

            entity.Likes = users.Select(el => new LikeModel
            {
                UserId = el.Item1,
                User = el.Item2
            });
        }

        private IEnumerable<Tuple<Guid, string>> GetManyNames(IEnumerable<Guid> usersIds)
        {
            var users = _intranetUserService.GetMany(usersIds);
            return users.Select(el => new Tuple<Guid, string>(el.Id, el.DisplayedName));
        }
    }
}