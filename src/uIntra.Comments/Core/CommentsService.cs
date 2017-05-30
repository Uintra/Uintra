using System;
using System.Collections.Generic;
using uIntra.Core.Persistence;

namespace uIntra.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly ISqlRepository<Guid, Comment> _commentsRepository;

        public CommentsService(ISqlRepository<Guid, Comment> commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public Comment Get(Guid id)
        {
            return _commentsRepository.Get(id);
        }

        public IEnumerable<Comment> GetMany(Guid activityId)
        {
            return _commentsRepository.FindAll(comment => comment.ActivityId == activityId);
        }

        public int GetCount(Guid activityId)
        {
            var count = _commentsRepository.Count(comment => comment.ActivityId == activityId);
            return (int)count;
        }

        public bool CanEdit(Comment comment, Guid editorId)
        {
            return comment.UserId == editorId;
        }

        public bool CanDelete(Comment comment, Guid editorId)
        {
            return comment.UserId == editorId;
        }

        public bool WasChanged(Comment comment)
        {
            return comment.CreatedDate != comment.ModifyDate;
        }

        public bool IsReply(Comment comment)
        {
            return comment.ParentId.HasValue;
        }

        public Comment Create(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var entity = new Comment
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                UserId = userId,
                Text = text,
                ParentId = parentId
            };

            entity.CreatedDate = entity.ModifyDate = DateTime.Now.ToUniversalTime();
            _commentsRepository.Add(entity);
            return entity;
        }

        public Comment Update(Guid id, string text)
        {
            var comment = _commentsRepository.Get(id);

            comment.ModifyDate = DateTime.Now.ToUniversalTime();
            comment.Text = text;
            _commentsRepository.Update(comment);
            return comment;
        }

        public void Delete(Guid id)
        {
            var comment = _commentsRepository.Get(id);

            if (comment.ParentId == null)
            {
                var replies = _commentsRepository.FindAll(c => c.ParentId == id);
                _commentsRepository.Delete(replies);
            }

            _commentsRepository.Delete(comment);
        }

        public void FillComments(ICommentable entity)
        {
            var comments = GetMany(entity.Id);
            entity.Comments = comments;
        }

        public string GetCommentViewId(Guid commentId)
        {
            return $"js-comment-view-{commentId}";
        }
    }
}