using System;
using System.Collections.Generic;

namespace Uintra.Tagging.UserTags
{
    public interface IUserTagRelationService
    {
        IEnumerable<Guid> GetForEntity(Guid entityId);
        IEnumerable<(Guid tagId, Guid entityId)> GetAll();

        void Add(Guid entityId, Guid tagId);
        void Add(Guid entityId, IEnumerable<Guid> tagIds);

        void Remove(Guid entityId, Guid tagId);
        void Remove(Guid entityId, IEnumerable<Guid> tagIds);
        void RemoveForTags(IEnumerable<Guid> tagIds);
    }
}