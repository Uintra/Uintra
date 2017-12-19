using System;
using System.Collections.Generic;

namespace uIntra.Tagging.UserTags
{
    public interface IUserTagRelationService
    {
        IEnumerable<Guid> GetForEntity(Guid entityId);
        IEnumerable<(Guid tagId, Guid entityId)> GetAll();

        void AddRelation(Guid entityId, Guid tagId);
        void AddRelations(Guid entityId, IEnumerable<Guid> tagIds);

        void RemoveRelation(Guid entityId, Guid tagId);
        void RemoveRelations(Guid entityId, IEnumerable<Guid> tagIds);
        void RemoveRelationsForTags(IEnumerable<Guid> tagIds);
    }
}