using System;
using uIntra.Comments;

namespace Compent.uIntra.Core.Comments
{
    public class CustomCommentableService : ICommentableService
    {
        private readonly ICommentsService _commentsService;

        public CustomCommentableService(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        public Comment CreateComment(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var comment = _commentsService.Create(userId, activityId, text, parentId);
            return comment;
        }

        public void UpdateComment(Guid id, string text)
        {
            _commentsService.Update(id, text);
        }

        public void DeleteComment(Guid id)
        {
            _commentsService.Delete(id);
        }

        public ICommentable GetCommentsInfo(Guid activityId)
        {
            var result = new CustomCommentable { Id = activityId };
            _commentsService.FillComments(result);

            return result;
        }
    }
}