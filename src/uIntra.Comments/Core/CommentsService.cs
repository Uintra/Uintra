using System;
using System.Linq;
using System.Collections.Generic;
using Compent.LinkPreview.Client;
using uIntra.Core.Extensions;
using uIntra.Core.LinkPreview;
using uIntra.Core.LinkPreview.Sql;
using uIntra.Core.Persistence;
using LinkPreview = uIntra.Core.LinkPreview.LinkPreview;

namespace uIntra.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly ISqlRepository<Guid, Comment> _commentsRepository;

        // TODO: move link preview logic to the separate service

        private readonly ISqlRepository<int, CommentToLinkPreviewEntity> _previewRelationRepository;
        private readonly ISqlRepository<int, LinkPreviewEntity> _previewRepository;
        private readonly LinkPreviewModelMapper _linkPreviewModelMapper;

        public CommentsService(ISqlRepository<Guid, Comment> commentsRepository, 
            ISqlRepository<int, CommentToLinkPreviewEntity> previewRelationRepository,
            ISqlRepository<int, LinkPreviewEntity> previewRepository, LinkPreviewModelMapper linkPreviewModelMapper)
        {
            _commentsRepository = commentsRepository;
            _previewRelationRepository = previewRelationRepository;
            _previewRepository = previewRepository;
            _linkPreviewModelMapper = linkPreviewModelMapper;
        }

        public void SaveLinkPreview(Guid commentId, int previewId)
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
            _previewRepository.Delete(preview => previewIds.Contains(preview.Id));
        }

        public virtual CommentModel Get(Guid id)
        {
            var comment = _commentsRepository.Get(id);
            return Map(comment);
        }

        private LinkPreview GetCommentsLinkPreview(Guid id)
        {
            var previewIds = _previewRelationRepository.FindAll(r => r.CommentId == id).Select(r => r.LinkPreviewId);
            var preview = _previewRepository.GetMany(previewIds).Select(_linkPreviewModelMapper.MapPreview).SingleOrDefault();
            return preview;
        }

        public virtual IEnumerable<CommentModel> GetMany(Guid activityId)
        {
            return _commentsRepository
                .FindAll(comment => comment.ActivityId == activityId)
                .Select(Map);
        }

        private CommentModel Map(Comment entity)
        {
            var model = entity.Map<CommentModel>();
            var preview = GetCommentsLinkPreview(entity.Id);
            model.LinkPreview = preview;
            return model;
        }

        public virtual int GetCount(Guid activityId)
        {
            var count = _commentsRepository.Count(comment => comment.ActivityId == activityId);
            return (int)count;
        }

        public virtual bool CanEdit(CommentModel comment, Guid editorId)
        {
            return comment.UserId == editorId;
        }

        public virtual bool CanDelete(CommentModel comment, Guid editorId)
        {
            return comment.UserId == editorId;
        }

        public virtual bool WasChanged(CommentModel comment)
        {
            return comment.CreatedDate != comment.ModifyDate;
        }

        public virtual bool IsReply(CommentModel comment)
        {
            return comment.ParentId.HasValue;
        }

        public virtual CommentModel Create(CommentDto dto)
        {
            var entity = Map(dto);
            entity.CreatedDate = entity.ModifyDate = DateTime.Now.ToUniversalTime();
            _commentsRepository.Add(entity);
            if (dto.LinkPreviewId.HasValue)
            {
                AddLinkPreview(entity.Id, dto.LinkPreviewId.Value);
            }
            return entity.Map<CommentModel>();
        }

        private Comment Map(CommentDto dto)
        {
            var entity = new Comment
            {
                Id = Guid.NewGuid(),
                ActivityId = dto.ActivityId,
                UserId = dto.UserId,
                Text = dto.Text,
                ParentId = dto.ParentId
            };

            return entity;
        }

        public virtual CommentModel Update(Guid id, string text, int? linkPreviewId)
        {
            var comment = _commentsRepository.Get(id);

            comment.ModifyDate = DateTime.Now.ToUniversalTime();
            comment.Text = text;
            _commentsRepository.Update(comment);

            if (linkPreviewId.HasValue)
            {
                SaveLinkPreview(id, linkPreviewId.Value);
            }
            else
            {
                RemovePreviewRelations(id);
            }

            return comment.Map<CommentModel>();
        }

        public virtual void Delete(Guid id)
        {
            var comment = _commentsRepository.Get(id);
            Delete(comment);
        }

        protected virtual void Delete(Comment comment)
        {
            if (comment.ParentId == null)
            {
                var replies = GetReplies(comment);
                foreach (var reply in replies)
                {
                    Delete(reply);
                }
            }

            _commentsRepository.Delete(comment);
            RemovePreviewRelations(comment.Id);
        }

        private IList<Comment> GetReplies(Comment comment)
        {
            return _commentsRepository.FindAll(c => c.ParentId == comment.Id);
        }

        public virtual void FillComments(ICommentable entity)
        {
            var comments = GetMany(entity.Id);
            entity.Comments = comments;
        }

        public virtual string GetCommentViewId(Guid commentId)
        {
            return $"js-comment-view-{commentId}";
        }

        public bool IsExistsUserComment(Guid activityId, Guid userId)
        {
            var result = _commentsRepository.Exists(c => c.ActivityId == activityId && c.UserId == userId);

            return result;

        }
    }
}