using System;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Features.Comments.Sql;
using Uintra20.Features.LinkPreview.Mappers;
using Uintra20.Features.LinkPreview.Sql;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Comments.Services
{
    public class CommentLinkPreviewService : ICommentLinkPreviewService
    {
        private readonly ISqlRepository<int, CommentToLinkPreviewEntity> _previewRelationRepository;
        private readonly ISqlRepository<int, LinkPreviewEntity> _previewRepository;
        private readonly LinkPreviewModelMapper _linkPreviewModelMapper;

        public CommentLinkPreviewService(
            ISqlRepository<int, CommentToLinkPreviewEntity> previewRelationRepository,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper)
        {
            _previewRelationRepository = previewRelationRepository;
            _previewRepository = previewRepository;
            _linkPreviewModelMapper = linkPreviewModelMapper;
        }

        #region async
        public async Task<LinkPreview.Models.LinkPreview> GetCommentsLinkPreviewAsync(Guid commentId)
        {
            //var previewIds = (await _previewRelationRepository.FindAllAsync(r => r.CommentId == commentId)).Select(r => r.LinkPreviewId);//TODO: Review bug
            var previewIds = _previewRelationRepository.FindAll(r => r.CommentId == commentId).Select(r => r.LinkPreviewId);
            var preview = (await _previewRepository.FindAllAsync(entity => previewIds.Contains(entity.Id))).Select(_linkPreviewModelMapper.MapPreview).SingleOrDefault();
            return preview;
        }

        public async Task UpdateLinkPreviewAsync(Guid commentId, int previewId)
        {
            await RemovePreviewRelationsAsync(commentId);
            await AddLinkPreviewAsync(commentId, previewId);
        }

        public async Task AddLinkPreviewAsync(Guid commentId, int previewId)
        {
            var entity = new CommentToLinkPreviewEntity { CommentId = commentId, LinkPreviewId = previewId };
            await _previewRelationRepository.AddAsync(entity);
        }

        public async Task RemovePreviewRelationsAsync(Guid commentId)
        {
            var relations = (await _previewRelationRepository.FindAllAsync(r => r.CommentId == commentId)).ToList();
            var previewIds = relations.Select(r => r.LinkPreviewId).ToList();
            await _previewRelationRepository.DeleteAsync(entity => relations.Select(x => x.Id).Contains(entity.Id));
            foreach (var previewId in previewIds)
            {
                await _previewRepository.DeleteAsync(previewId);
            }
        }

        #endregion

        #region sync

        public LinkPreview.Models.LinkPreview GetCommentsLinkPreview(Guid commentId)
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

        #endregion
    }
}