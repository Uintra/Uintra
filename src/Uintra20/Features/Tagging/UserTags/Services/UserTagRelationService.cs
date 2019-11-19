using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using LanguageExt;
using Uintra20.Features.Tagging.UserTags.Sql;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Tagging.UserTags.Services
{
    public class UserTagRelationService : IUserTagRelationService
    {
        private readonly ISqlRepository<int, UserTagRelation> _relationRepository;

        public UserTagRelationService(ISqlRepository<int, UserTagRelation> relationRepository)
        {
            _relationRepository = relationRepository;
        }

        public async Task<IEnumerable<Guid>> GetForEntityAsync(Guid entityId)
        {
            return await _relationRepository
                .FindAllAsync(r => r.EntityId == entityId)
                .Select(r => r.Select(x => x.UserTagId));
        }

        public async Task<IEnumerable<(Guid tagId, Guid entityId)>> GetAllAsync()
        {
            return await _relationRepository.GetAllAsync().Select(r => r.Select(x => (x.UserTagId, x.EntityId)));
        }

        public async Task AddAsync(Guid entityId, Guid tagId)
        {
            await AddAsync(entityId, tagId.ToEnumerable());
        }

        public async Task AddAsync(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var entities = tagIds.Select(tagId => MapToEntity(entityId, tagId));
            await _relationRepository.AddAsync(entities);
        }

        public async Task RemoveAsync(Guid entityId, Guid tagId)
        {
            await RemoveAsync(entityId, tagId.ToEnumerable());
        }

        public async Task RemoveAsync(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var tagIdsList = tagIds.AsList();
            if (tagIdsList.IsEmpty()) return;
            var uniqueTagIds = tagIdsList.Distinct().AsList();
            await _relationRepository.DeleteAsync(e => uniqueTagIds.Contains(e.UserTagId) && e.EntityId == entityId);
        }

        public async Task RemoveForTagsAsync(IEnumerable<Guid> tagIds)
        {
            var tagIdsList = tagIds.AsList();
            if (tagIdsList.IsEmpty()) return;
            var uniqueTagIds = tagIdsList.Distinct().AsList();
            await _relationRepository.DeleteAsync(rel => uniqueTagIds.Contains(rel.UserTagId));
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