using System;
using System.Collections.Generic;
using uCommunity.Core.App_Plugins.Core.Persistence.Sql;
using uCommunity.Likes.App_Plugins.Likes.Sql;

namespace uCommunity.Likes.App_Plugins.Likes
{
    public class LikesService : ILikesService
    {
        private readonly ISqlRepository<Like> _likesRepository;

        public LikesService(ISqlRepository<Like> likesRepository)
        {
            _likesRepository = likesRepository;
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

        public bool CanRemove(Guid userId, Guid activityId)
        {
            return !CanAdd(userId, activityId);
        }
    }
}