using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;

namespace uIntra.Tagging.UserTags
{
    public class UserTagRelationService : IUserTagRelationService
    {
        private readonly ISqlRepository<int, UserTagRelation> _relationRepository;

        public UserTagRelationService(ISqlRepository<int, UserTagRelation> relationRepository)
        {
            _relationRepository = relationRepository;
        }

        public IEnumerable<Guid> GetForEntity(Guid entityId)
        {
            return _relationRepository
                .FindAll(r => r.EntityId == entityId)
                .Select(r => r.UserTagId);
        }

        public IEnumerable<(Guid tagId, Guid entityId)> GetAll()
        {
            return _relationRepository.GetAll().Select(r => (r.UserTagId, r.EntityId));
        }

        public void AddRelation(Guid entityId, Guid tagId)
        {
            AddRelations(entityId, tagId.ToEnumerableOfOne());
        }

        public void RemoveRelation(Guid entityId, Guid tagId)
        {
            RemoveRelations(entityId, tagId.ToEnumerableOfOne());
        }

        public void AddRelations(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var entities = tagIds.Select(tagId => MapToEntity(entityId, tagId));
            _relationRepository.Add(entities);
        }

        private UserTagRelation MapToEntity(Guid entityId, Guid tagId)
        {
            return new UserTagRelation
            {
                UserTagId = tagId,
                EntityId = entityId
            };
        }

        public void RemoveRelations(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var tagIdHashSet = new HashSet<Guid>(tagIds);
            _relationRepository.Delete(e => tagIdHashSet.Contains(e.UserTagId) && e.EntityId == entityId);
        }

        public void RemoveRelationsForTags(IEnumerable<Guid> tagIds)
        {
            var tagIdHashSet = new HashSet<Guid>(tagIds);
            _relationRepository.Delete(rel => tagIdHashSet.Contains(rel.UserTagId));
        }
    }
}