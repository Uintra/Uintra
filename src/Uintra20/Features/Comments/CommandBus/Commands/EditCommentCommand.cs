using System;
using Uintra20.Features.Comments.Models;

namespace Uintra20.Features.Comments.CommandBus.Commands
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