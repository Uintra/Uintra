using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Extensions;
using Uintra20.Persistence;

namespace Uintra20.Core.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly ISqlRepository<Guid, Comment> _commentsRepository;
        private readonly ICommentLinkPreviewService _commentLinkPreviewService;

        public CommentsService(ICommentLinkPreviewService commentLinkPreviewService, ISqlRepository<Guid, Comment> commentsRepository)
        {
            _commentLinkPreviewService = commentLinkPreviewService;
            _commentsRepository = commentsRepository;
        }

        #region async

        public virtual async Task<CommentModel> GetAsync(Guid id)
        {
            var comment = await _commentsRepository.GetAsync(id);
            return await MapAsync(comment);
        }

        public virtual async Task<IEnumerable<CommentModel>> GetManyAsync(Guid activityId)
        {
            return await (await _commentsRepository
                .FindAllAsync(comment => comment.ActivityId == activityId))
                .SelectAsync(MapAsync);
        }

        private async Task<CommentModel> MapAsync(Comment entity)
        {
            var model = entity.Map<CommentModel>();
            var preview = await _commentLinkPreviewService.GetCommentsLinkPreviewAsync(entity.Id);
            model.LinkPreview = preview;
            return model;
        }

        public virtual async Task<int> GetCountAsync(Guid activityId)
        {
            var count = await _commentsRepository.CountAsync(comment => comment.ActivityId == activityId);
            return (int)count;
        }

        public async Task<bool> IsExistsUserCommentAsync(Guid activityId, Guid userId) =>
            await _commentsRepository.ExistsAsync(c => c.ActivityId == activityId && c.UserId == userId);

        public virtual async Task<CommentModel> UpdateAsync(CommentEditDto dto)
        {
            var comment = await _commentsRepository.GetAsync(dto.Id);
            var commentLinkPreview = _commentLinkPreviewService.GetCommentsLinkPreviewAsync(comment.Id);

            comment.ModifyDate = DateTime.UtcNow;
            comment.Text = dto.Text;

            await _commentsRepository.UpdateAsync(comment);

            if (dto.LinkPreviewId.HasValue)
            {
                if (commentLinkPreview?.Id != dto.LinkPreviewId)
                {
                    await _commentLinkPreviewService.UpdateLinkPreviewAsync(dto.Id, dto.LinkPreviewId.Value);
                }
            }
            else
            {
                await _commentLinkPreviewService.RemovePreviewRelationsAsync(dto.Id);
            }

            return comment.Map<CommentModel>();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var comment = await _commentsRepository.GetAsync(id);
            await DeleteAsync(comment);
        }

        protected virtual async Task DeleteAsync(Comment comment)
        {
            async Task<IEnumerable<Comment>> GetDescendants(Comment current) =>
                (await (await _commentsRepository
                        .FindAllAsync(c => c.ParentId == current.Id))
                    .SelectManyAsync(GetDescendants))
                .Append(current);

            var commentsToDelete = await GetDescendants(comment);

            commentsToDelete.Iter(async c =>
            {
                await _commentLinkPreviewService.RemovePreviewRelationsAsync(c.Id);
                await _commentsRepository.DeleteAsync(c);
            });
        }

        public virtual async Task FillCommentsAsync(ICommentable entity)
        {
            var comments = await GetManyAsync(entity.Id);
            entity.Comments = comments;
        }

        public virtual async Task<CommentModel> CreateAsync(CommentCreateDto dto)
        {
            var entity = Map(dto);
            entity.CreatedDate = entity.ModifyDate = DateTime.UtcNow;
            await _commentsRepository.AddAsync(entity);
            if (dto.LinkPreviewId.HasValue)
            {
                await _commentLinkPreviewService.AddLinkPreviewAsync(entity.Id, dto.LinkPreviewId.Value);
            }
            return entity.Map<CommentModel>();
        }

        #endregion

        #region sync

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
            entity.CreatedDate = entity.ModifyDate = DateTime.UtcNow;
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

            comment.ModifyDate = DateTime.UtcNow;
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

        #endregion
    }
}