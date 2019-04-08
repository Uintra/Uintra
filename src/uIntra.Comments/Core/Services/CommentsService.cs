using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;

namespace Uintra.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly ISqlRepository<Guid, Comment> _commentsRepository;
        private readonly ICommentLinkPreviewService _commentLinkPreviewService;

        public CommentsService(ISqlRepository<Guid, Comment> commentsRepository, ICommentLinkPreviewService commentLinkPreviewService)
        {
            _commentsRepository = commentsRepository;
            _commentLinkPreviewService = commentLinkPreviewService;
        }

        public virtual CommentModel Get(Guid id)
        {
            var comment = _commentsRepository.Get(id);
            return Map(comment);
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
            var preview = _commentLinkPreviewService.GetCommentsLinkPreview(entity.Id);
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

        public virtual CommentModel Create(CommentCreateDto dto)
        {
            var entity = Map(dto);
            entity.CreatedDate = entity.ModifyDate = DateTime.Now.ToUniversalTime();
            _commentsRepository.Add(entity);
            if (dto.LinkPreviewId.HasValue)
            {
                _commentLinkPreviewService.AddLinkPreview(entity.Id, dto.LinkPreviewId.Value);
            }
            return entity.Map<CommentModel>();
        }

        private static Comment Map(CommentCreateDto dto) =>
            new Comment
            {
                Id = dto.Id,
                ActivityId = dto.ActivityId,
                UserId = dto.UserId,
                Text = dto.Text,
                ParentId = dto.ParentId
            };

        public virtual CommentModel Update(CommentEditDto dto)
        {
            var comment = _commentsRepository.Get(dto.Id);
            var commentLinkPreview = _commentLinkPreviewService.GetCommentsLinkPreview(comment.Id);

            comment.ModifyDate = DateTime.Now.ToUniversalTime();
            comment.Text = dto.Text;

            _commentsRepository.Update(comment);

            if (dto.LinkPreviewId.HasValue)
            {
                if (commentLinkPreview?.Id != dto.LinkPreviewId)
                {
                    _commentLinkPreviewService.UpdateLinkPreview(dto.Id, dto.LinkPreviewId.Value);
                }
            }
            else
            {
                _commentLinkPreviewService.RemovePreviewRelations(dto.Id);
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
            IEnumerable<Comment> GetDescendants(Comment current) =>
                    _commentsRepository
                    .FindAll(c => c.ParentId == current.Id)
                    .SelectMany(GetDescendants)
                    .Append(current);

            var commentsToDelete = GetDescendants(comment);

            commentsToDelete.Iter(c =>
            {
                _commentLinkPreviewService.RemovePreviewRelations(c.Id);
                _commentsRepository.Delete(c);
            });
        }

        public virtual void FillComments(ICommentable entity)
        {
            var comments = GetMany(entity.Id);
            entity.Comments = comments;
        }

        public virtual string GetCommentViewId(Guid commentId) =>
            $"js-comment-view-{commentId}";

        public bool IsExistsUserComment(Guid activityId, Guid userId) =>
            _commentsRepository.Exists(c => c.ActivityId == activityId && c.UserId == userId);
    }
}