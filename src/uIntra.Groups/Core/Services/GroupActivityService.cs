using System;
using uIntra.Core.Persistence;
using uIntra.Groups.Sql;

namespace uIntra.Groups
{
    public class GroupActivityService : IGroupActivityService
    {
        private readonly ISqlRepository<GroupActivityRelation> _groupActivityRepository;

        public GroupActivityService(ISqlRepository<GroupActivityRelation> groupActivityRepository)
        {
            _groupActivityRepository = groupActivityRepository;
        }

        public void AddRelation(Guid groupId, Guid activityId)
        {
            var relation = new GroupActivityRelation
            {
                ActivityId = activityId,
                GroupId = groupId,
                Id = Guid.NewGuid()
            };

            _groupActivityRepository.Add(relation);
        }

        public void RemoveRelation(Guid groupId, Guid activityId)
        {
            _groupActivityRepository.Delete(r => r.ActivityId.Equals(activityId) && r.GroupId.Equals(groupId));
        }

        public Guid? GetGroupId(Guid activityId)
        {
            return _groupActivityRepository.Find(rel => rel.ActivityId == activityId)?.GroupId;
        }
    }
}