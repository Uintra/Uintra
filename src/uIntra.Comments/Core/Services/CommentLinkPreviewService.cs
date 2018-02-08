using System;
using System.Linq;
using uIntra.Core.LinkPreview;
using uIntra.Core.LinkPreview.Sql;
using uIntra.Core.Persistence;

namespace uIntra.Comments
{
    public class CommentLinkPreviewService : ICommentLinkPreviewService
    {
        private readonly ISqlRepository<int, CommentToLinkPreviewEntity> _previewRelationRepository;
        private readonly ISqlRepository<int, LinkPreviewEntity> _previewRepository;
        private readonly LinkPreviewModelMapper _linkPreviewModelMapper;

        public CommentLinkPreviewService(ISqlRepository<int, CommentToLinkPreviewEntity> previewRelationRepository, ISqlRepository<int, LinkPreviewEntity> previewRepository, LinkPreviewModelMapper linkPreviewModelMapper)
        {
            _previewRelationRepository = previewRelationRepository;
            _previewRepository = previewRepository;
            _linkPreviewModelMapper = linkPreviewModelMapper;
        }

        public LinkPreview GetCommentsLinkPreview(Guid commentId)
        {
            var previewIds = _previewRelationRepository.FindAll(r => r.CommentId == commentId).Select(r => r.LinkPreviewId);
            var preview = _previewRepository.GetMany(previewIds).Select(_linkPreviewModelMapper.MapPreview).SingleOrDefault();
            return preview;
        }

        public void UpdateLinkPreview(Guid commentId, int previewId)
        {
            RemovePreviewRelations(commentId);
            AddLinkPreview(commentId, previewId);
        }

        public void AddLinkPreview(Guid commentId, int previewId)
        {
            var entity = new CommentToLinkPreviewEntity { CommentId = commentId, LinkPreviewId = previewId };
            _previewRelationRepository.Add(entity);
        }

        public void RemovePreviewRelations(Guid commentId)
        {
            var relations = _previewRelationRepository.FindAll(r => r.CommentId == commentId).ToList();
            var previewIds = relations.Select(r => r.LinkPreviewId).ToList();
            _previewRelationRepository.Delete(relations);
            foreach (var previewId in previewIds)
            {
                _previewRepository.Delete(previewId);
            }
        }
    }
}