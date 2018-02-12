using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Persistence;

namespace Uintra.Core.Media
{
    public class IntranetMediaService: IIntranetMediaService
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

        public string GetEntityMediaString(Guid entityId)
        {
            var entity = _sqlRepository.Find(e => e.EntityId == entityId);
            return !string.IsNullOrEmpty(entity?.MediaIds) ? entity.MediaIds : string.Empty;
        }

        public void Update(Guid entityId, string mediaIds)
        {
            if (string.IsNullOrEmpty(mediaIds))
            {
                Delete(entityId);
                return;
            }

            var mediaEntity = _sqlRepository.Find(e=>e.EntityId == entityId);
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
            _sqlRepository.Delete(m=>m.EntityId == entityId);
        }
    }
}
