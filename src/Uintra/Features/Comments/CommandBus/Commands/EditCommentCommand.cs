using System;
using Uintra.Features.Comments.Models;

namespace Uintra.Features.Comments.CommandBus.Commands
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