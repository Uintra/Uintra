using System;
using System.Linq;
using System.Collections.Generic;
using Compent.LinkPreview.Client;
using uIntra.Core.Extensions;
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
        private readonly IUriProvider _uriProvider;

        public CommentsService(ISqlRepository<Guid, Comment> commentsRepository, 
            ISqlRepository<int, CommentToLinkPreviewEntity> previewRelationRepository,
            ISqlRepository<int, LinkPreviewEntity> previewRepository, IUriProvider uriProvider)
        {
            _commentsRepository = commentsRepository;
            _previewRelationRepository = previewRelationRepository;
            _previewRepository = previewRepository;
            _uriProvider = uriProvider;
        }

        public virtual CommentModel Get(Guid id)
        {
            var comment = _commentsRepository.Get(id);
            return Map(comment);
        }

        private LinkPreview GetCommentsLinkPreview(Guid id)
        {
            var previewIds = _previewRelationRepository.FindAll(r => r.CommentId == id).Select(r => r.LinkPreviewId);
            var preview = _previewRepository.GetMany(previewIds).Select(MapPreview).SingleOrDefault();
            return preview;
        }

        private LinkPreview MapPreview(LinkPreviewEntity entity)
        {
            var result = new LinkPreview
            {
                Title = entity.Title,
                Description = GetLongest(entity.OgDescription, entity.Description),
                ImageUri = _uriProvider.GetImageUri(entity.ImageId),
                FaviconUri = _uriProvider.GetImageUri(entity.FaviconId)
            };

            return result;
        }

        private static string GetLongest(string first, string second) =>
            GetNullableLength(first) > GetNullableLength(second) ? first : second;

        private static int GetNullableLength(string str) =>
            str?.Length ?? default;

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

        public virtual CommentModel Create(Guid userId, Guid activityId, string text, Guid? parentId)
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
            return entity.Map<CommentModel>();
        }

        public virtual CommentModel Update(Guid id, string text)
        {
            var comment = _commentsRepository.Get(id);

            comment.ModifyDate = DateTime.Now.ToUniversalTime();
            comment.Text = text;
            _commentsRepository.Update(comment);
            return comment.Map<CommentModel>();
        }

        public virtual void Delete(Guid id)
        {
            var comment = _commentsRepository.Get(id);

            if (comment.ParentId == null)
            {
                var replies = _commentsRepository.FindAll(c => c.ParentId == id);
                _commentsRepository.Delete(replies);
            }

            _commentsRepository.Delete(comment);
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