using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Features.Tagging.UserTags.Services
{
    public interface IUserTagRelationService
    {
        Task<IEnumerable<Guid>> GetForEntityAsync(Guid entityId);
        Task<IEnumerable<(Guid tagId, Guid entityId)>> GetAllAsync();
        Task AddAsync(Guid entityId, Guid tagId);
        Task AddAsync(Guid entityId, IEnumerable<Guid> tagIds);
        Task RemoveAsync(Guid entityId, Guid tagId);
        Task RemoveAsync(Guid entityId, IEnumerable<Guid> tagIds);
        Task RemoveForTagsAsync(IEnumerable<Guid> tagIds);

        IEnumerable<Guid> GetForEntity(Guid entityId);
        IEnumerable<(Guid tagId, Guid entityId)> GetAll();
        void Add(Guid entityId, Guid tagId);
        void Add(Guid entityId, IEnumerable<Guid> tagIds);
        void Remove(Guid entityId, Guid tagId);
        void Remove(Guid entityId, IEnumerable<Guid> tagIds);
        void RemoveForTags(IEnumerable<Guid> tagIds);
    }
}
