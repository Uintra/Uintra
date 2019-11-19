using System;
using Uintra20.Features.Comments.Models;

namespace Uintra20.Features.Comments.CommandBus.Commands
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