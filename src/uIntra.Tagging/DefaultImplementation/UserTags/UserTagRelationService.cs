using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Uintra.Core.Persistence;

namespace Uintra.Tagging.UserTags
{
    public class UserTagRelationService : IUserTagRelationService
    {
        private readonly ISqlRepository<int, UserTagRelation> _relationRepository;

        public UserTagRelationService(ISqlRepository<int, UserTagRelation> relationRepository)
        {
            _relationRepository = relationRepository;
        }

        public virtual IEnumerable<Guid> GetForEntity(Guid entityId)
        {
            return _relationRepository
                .FindAll(r => r.EntityId == entityId)
                .Select(r => r.UserTagId);
        }

        public virtual IEnumerable<(Guid tagId, Guid entityId)> GetAll()
        {
            return _relationRepository.GetAll().Select(r => (r.UserTagId, r.EntityId));
        }

        public virtual void Add(Guid entityId, Guid tagId)
        {
            Add(entityId, tagId.ToEnumerable());
        }

        public virtual void Remove(Guid entityId, Guid tagId)
        {
            Remove(entityId, tagId.ToEnumerable());
        }

        public virtual void Add(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var entities = tagIds.Select(tagId => MapToEntity(entityId, tagId));
            _relationRepository.Add(entities);
        }

        protected virtual UserTagRelation MapToEntity(Guid entityId, Guid tagId)
        {
            return new UserTagRelation
            {
                UserTagId = tagId,
                EntityId = entityId
            };
        }

        public virtual void Remove(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var tagIdsList = tagIds.AsList();
            if (tagIdsList.IsEmpty()) return;
            var uniqueTagIds = tagIdsList.Distinct().AsList();
            _relationRepository.Delete(e => uniqueTagIds.Contains(e.UserTagId) && e.EntityId == entityId);
        }

        public virtual void RemoveForTags(IEnumerable<Guid> tagIds)
        {
            var tagIdsList = tagIds.AsList();
            if (tagIdsList.IsEmpty()) return;
            var uniqueTagIds = tagIdsList.Distinct().AsList();
            _relationRepository.Delete(rel => uniqueTagIds.Contains(rel.UserTagId));
        }
    }
}