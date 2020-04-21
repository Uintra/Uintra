using System;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Features.LinkPreview.Sql;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.LinkPreview
{
    public class ActivityLinkPreviewService : IActivityLinkPreviewService
    {
        private readonly ISqlRepository<int, ActivityToLinkPreviewEntity> _previewRelationRepository;
        private readonly ISqlRepository<int, LinkPreviewEntity> _previewRepository;
        private readonly LinkPreviewModelMapper _linkPreviewModelMapper;

        public ActivityLinkPreviewService(
            ISqlRepository<int, ActivityToLinkPreviewEntity> previewRelationRepository,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper)
        {
            _previewRelationRepository = previewRelationRepository;
            _previewRepository = previewRepository;
            _linkPreviewModelMapper = linkPreviewModelMapper;
        }

        public async Task<Models.LinkPreview> GetActivityLinkPreviewAsync(Guid activityId)
        {
            var previewIds = (await _previewRelationRepository.FindAllAsync(r => r.ActivityId == activityId)).Select(r => r.LinkPreviewId);
            var preview = (await _previewRepository.GetManyAsync(previewIds)).Select(_linkPreviewModelMapper.MapPreview).SingleOrDefault();
            return preview;
        }

        public async Task RemovePreviewRelationsAsync(Guid activityId)
        {
            var relations = (await _previewRelationRepository.FindAllAsync(r => r.ActivityId == activityId)).ToList();
            var previewIds = relations.Select(r => r.LinkPreviewId).ToList();
            await _previewRelationRepository.DeleteAsync(relations);
            foreach (var previewId in previewIds)
            {
                if (await _previewRepository.ExistsAsync(p => p.Id == previewId))
                {
                    await _previewRepository.DeleteAsync(previewId);
                }
            }
        }

        public async Task AddLinkPreviewAsync(Guid activityId, int previewId)
        {
            var entity = new ActivityToLinkPreviewEntity { ActivityId = activityId, LinkPreviewId = previewId };
            await _previewRelationRepository.AddAsync(entity);
        }

        public async Task UpdateLinkPreviewAsync(Guid activityId, int previewId)
        {
            var entities = await _previewRelationRepository.FindAllAsync(r => r.ActivityId == activityId);
            
            await _previewRelationRepository.DeleteAsync(entities);

            await AddLinkPreviewAsync(activityId, previewId);
        }


        public Models.LinkPreview GetActivityLinkPreview(Guid activityId)
        {
            var previewIds = _previewRelationRepository.FindAll(r => r.ActivityId == activityId).Select(r => r.LinkPreviewId);
            var preview = _previewRepository.GetMany(previewIds).Select(_linkPreviewModelMapper.MapPreview).SingleOrDefault();
            return preview;
        }

        public void UpdateLinkPreview(Guid activityId, int previewId)
        {
            var relations = _previewRelationRepository.FindAll(r => r.ActivityId == activityId).ToList();
            _previewRelationRepository.Delete(relations);
            AddLinkPreview(activityId, previewId);
        }

        public void AddLinkPreview(Guid activityId, int previewId)
        {
            var entity = new ActivityToLinkPreviewEntity { ActivityId = activityId, LinkPreviewId = previewId };
            _previewRelationRepository.Add(entity);
        }

        public void RemovePreviewRelations(Guid activityId)
        {
            var relations = _previewRelationRepository.FindAll(r => r.ActivityId == activityId).ToList();
            var previewIds = relations.Select(r => r.LinkPreviewId).ToList();
            _previewRelationRepository.Delete(relations);
            foreach (var previewId in previewIds)
            {
                if (_previewRepository.Exists(p => p.Id == previewId))
                {
                    _previewRepository.Delete(previewId);
                }
            }
        }
    }
}