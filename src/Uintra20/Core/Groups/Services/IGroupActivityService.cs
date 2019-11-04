using System;
using System.Threading.Tasks;

namespace Uintra20.Core.Groups.Services
{
    public interface IGroupActivityService
    {
        void AddRelation(Guid groupId, Guid activityId);
        void RemoveRelation(Guid groupId, Guid activityId);
        Guid? GetGroupId(Guid activityId);

        Task AddRelationAsync(Guid groupId, Guid activityId);
        Task RemoveRelationAsync(Guid groupId, Guid activityId);
        Task<Guid?> GetGroupIdAsync(Guid activityId);
    }
}
