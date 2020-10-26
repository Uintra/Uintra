using System;
using Uintra.Features.Comments.Models;

namespace Uintra.Features.Comments.CommandBus.Commands
{
    public class AddCommentCommand : CommentCommand
    {
        public CommentCreateDto CreateDto { get; }

        public AddCommentCommand(Guid targetId, Enum targetType, CommentCreateDto createDto) : base(targetId, targetType)
        {
            CreateDto = createDto;
        }
    }
}