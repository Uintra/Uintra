using System;

namespace Uintra20.Core.Comments.CommandBus
{
    public class EditCommentCommand : CommentCommand
    {
        public CommentEditDto EditDto { get; }

        public EditCommentCommand(Guid targetId, Enum targetType, CommentEditDto editDto) : base(targetId, targetType)
        {
            EditDto = editDto;
        }
    }
}