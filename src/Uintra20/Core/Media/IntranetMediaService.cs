using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Uintra20.Core.Media.Sql;
using Uintra20.Persistence;

namespace Uintra20.Core.Media
{
    public class IntranetMediaService : IIntranetMediaService
    {
        private readonly ISqlRepository<Guid, IntranetMediaEntity> _sqlRepository;

        public IntranetMediaService(ISqlRepository<Guid, IntranetMediaEntity> sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public void Create(Guid entityId, string mediaIds)
        {
            if (!string.IsNullOrEmpty(mediaIds) && !string.IsNullOrWhiteSpace(mediaIds))
            {
                _sqlRepository.Add(new IntranetMediaEntity
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    MediaIds = mediaIds
                });
            }
        }

        public IEnumerable<int> GetEntityMedia(Guid entityId)
        {
            var entity = _sqlRepository.Find(e => e.EntityId == entityId);
            return !string.IsNullOrEmpty(entity?.MediaIds)
                ? entity.MediaIds.Split(',').Select(id => Convert.ToInt32(id))
                : Enumerable.Empty<int>();
        }

        public Guid GetEntityIdByMediaId(int mediaId)
        {
            // todo: refactor to store relation entity to mediaId in separate 
            return _sqlRepository.Find(a => a.MediaIds.Contains(mediaId.ToString())).EntityId;
        }

        public string GetEntityMediaString(Guid entityId)
        {
            var entity = _sqlRepository.Find(e => e.EntityId == entityId);
            return !string.IsNullOrEmpty(entity?.MediaIds) ? entity.MediaIds : string.Empty;
        }

        public async Task CreateAsync(Guid entityId, string mediaIds)
        {
            if (!string.IsNullOrEmpty(mediaIds) && !string.IsNullOrWhiteSpace(mediaIds))
            {
                await _sqlRepository.AddAsync(new IntranetMediaEntity
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    MediaIds = mediaIds
                });
            }
        }

        public async Task<IEnumerable<int>> GetEntityMediaAsync(Guid entityId)
        {
            var entity = await _sqlRepository.FindAsync(e => e.EntityId == entityId);
            return !string.IsNullOrEmpty(entity?.MediaIds)
                ? entity.MediaIds.Split(',').Select(id => Convert.ToInt32(id))
                : Enumerable.Empty<int>();
        }

        public async Task DeleteAsync(Guid entityId)
        {
            await _sqlRepository.DeleteAsync(m => m.EntityId == entityId);
        }

        public async Task UpdateAsync(Guid entityId, string mediaIds)
        {
            if (string.IsNullOrEmpty(mediaIds))
            {
                await DeleteAsync(entityId);
                return;
            }

            var mediaEntity = await _sqlRepository.FindAsync(e => e.EntityId == entityId);
            if (mediaEntity == null)
            {
                await CreateAsync(entityId, mediaIds);
            }
            else
            {
                mediaEntity.MediaIds = mediaIds;
                await _sqlRepository.UpdateAsync(mediaEntity);
            }
        }

        public async Task<Guid> GetEntityIdByMediaIdAsync(int mediaId)
        {
            // todo: refactor to store relation entity to mediaId in separate 
            return await _sqlRepository.FindAsync(a => a.MediaIds.Contains(mediaId.ToString())).Select(x => x.EntityId);
        }

        public async Task<string> GetEntityMediaStringAsync(Guid entityId)
        {
            var entity = await _sqlRepository.FindAsync(e => e.EntityId == entityId);
            return !string.IsNullOrEmpty(entity?.MediaIds) ? entity.MediaIds : string.Empty;
        }

        public void Update(Guid entityId, string mediaIds)
        {
            if (string.IsNullOrEmpty(mediaIds))
            {
                Delete(entityId);
                return;
            }

            var mediaEntity = _sqlRepository.Find(e => e.EntityId == entityId);
            if (mediaEntity == null)
            {
                Create(entityId, mediaIds);
            }
            else
            {
                mediaEntity.MediaIds = mediaIds;
                _sqlRepository.Update(mediaEntity);
            }

        }

        public void Delete(Guid entityId)
        {
            _sqlRepository.Delete(m => m.EntityId == entityId);
        }
    }
}