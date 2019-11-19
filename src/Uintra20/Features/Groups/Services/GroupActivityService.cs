using System;
using System.Threading.Tasks;
using Uintra20.Features.Groups.Sql;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Groups.Services
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

        public async Task AddRelationAsync(Guid groupId, Guid activityId)
        {
            var relation = new GroupActivityRelation
            {
                ActivityId = activityId,
                GroupId = groupId,
                Id = Guid.NewGuid()
            };

            await _groupActivityRepository.AddAsync(relation);
        }

        public async Task RemoveRelationAsync(Guid groupId, Guid activityId)
        {
            await _groupActivityRepository.DeleteAsync(r => r.ActivityId.Equals(activityId) && r.GroupId.Equals(groupId));
        }

        public async Task<Guid?> GetGroupIdAsync(Guid activityId)
        {
            return (await _groupActivityRepository.FindAsync(rel => rel.ActivityId == activityId))?.GroupId;
        }
    }
}