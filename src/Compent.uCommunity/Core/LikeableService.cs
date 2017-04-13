namespace Compent.uCommunity.Core
{
    //public class LikeableService : ILikeableService
    //{
    //    private readonly ISqlRepository<Like> _likesRepository;

    //    public LikeableService(ISqlRepository<Like> likesRepository)
    //    {
    //        _likesRepository = likesRepository;
    //    }

    //    public IEnumerable<LikeModel> GetLikes(Guid activityId)
    //    {
    //        var likes =  _likesRepository.FindAll(l => l.ActivityId == activityId);
    //        return likes.Select(l => new LikeModel {UserId = l.UserId, User = "user"});
    //    }

    //    public int GetCount(Guid activityId)
    //    {
    //        return (int)_likesRepository.Count(l => l.ActivityId == activityId);
    //    }

    //    public void Add(Guid userId, Guid activityId)
    //    {
    //        var like = new Like
    //        {
    //            Id = Guid.NewGuid(),
    //            ActivityId = activityId,
    //            UserId = userId,
    //            CreatedDate = DateTime.Now
    //        };

    //        _likesRepository.Add(like);
    //    }

    //    public void Remove(Guid userId, Guid activityId)
    //    {
    //        _likesRepository.Delete(like => like.ActivityId == activityId && like.UserId == userId);
    //    }

    //    public bool CanAdd(Guid userId, Guid activityId)
    //    {
    //        var exists = _likesRepository.Exists(like => like.ActivityId == activityId && like.UserId == userId);
    //        return !exists;
    //    }
    //}
}