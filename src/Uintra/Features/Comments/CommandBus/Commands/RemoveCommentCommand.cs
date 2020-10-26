using System;

namespace Uintra.Features.Comments.CommandBus.Commands
{
    public class RemoveCommentCommand : CommentCommand
    {
        public Guid CommentId { get; }

        public RemoveCommentCommand(Guid targetId, Enum targetType, Guid commentId) : base(targetId, targetType)
        {
            CommentId = commentId;
        }
    }
}