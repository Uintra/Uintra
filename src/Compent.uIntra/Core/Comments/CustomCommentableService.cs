using System;
using Uintra.Comments;

namespace Compent.Uintra.Core.Comments
{
    public class CustomCommentableService : ICustomCommentableService
    {

        // TODO: remove this contract after decoupling comments from activities
        public Enum ActivityType { get; } = DummyType.CustomCommentableService;
        private enum DummyType
        {
            CustomCommentableService = Int32.MaxValue
        }

        private readonly ICommentsService _commentsService;

        public CustomCommentableService(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        public CommentModel CreateComment(CommentCreateDto dto)
        {
            var comment = _commentsService.Create(dto);
            return comment;
        }

        public void UpdateComment(CommentEditDto dto)
        {
            _commentsService.Update(dto);
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