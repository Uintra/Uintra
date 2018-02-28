using System;
using System.Linq;
using Uintra.Core.LinkPreview.Sql;
using Uintra.Core.Persistence;

namespace Uintra.Core.LinkPreview
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

        public LinkPreview GetActivityLinkPreview(Guid activityId)
        {
            var previewIds = _previewRelationRepository.FindAll(r => r.ActivityId == activityId).Select(r => r.LinkPreviewId);
            var preview = _previewRepository.GetMany(previewIds).Select(_linkPreviewModelMapper.MapPreview).SingleOrDefault();
            return preview;
        }

        public void UpdateLinkPreview(Guid activityId, int previewId)
        {
            RemovePreviewRelations(activityId);
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
                _previewRepository.Delete(previewId);
            }
        }
    }
}